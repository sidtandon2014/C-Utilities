# Databricks notebook source
# MAGIC %md
# MAGIC 
# MAGIC #### Read data from ADLS

# COMMAND ----------

display(dbutils.fs.mounts())

# COMMAND ----------

trip_data = spark.read.format("csv").option("header", True).load("dbfs:/mnt/nyctaxi/source/tripdata/*.csv")

# COMMAND ----------

trip_fare = spark.read.format("csv").option("header", True).load("dbfs:/mnt/nyctaxi/source/trip_fare/*.csv")

# COMMAND ----------

display(trip_data)

# COMMAND ----------

display(trip_fare)

# COMMAND ----------

trip_data.printSchema()

# COMMAND ----------

trip_fare.printSchema()

# COMMAND ----------

trip_data.createOrReplaceTempView("vwTripData")
trip_fare.createOrReplaceTempView("vwTripFare")

# COMMAND ----------

# MAGIC %scala
# MAGIC 
# MAGIC spark.sql("""
# MAGIC SELECT 
# MAGIC   f.medallion
# MAGIC   ,f.` hack_license` AS hack_license
# MAGIC   , CASE f.` payment_type` WHEN 'CSH' THEN 1 WHEN 'CRD' THEN 2 ELSE 3 END payment_type
# MAGIC   ,CAST(f.` pickup_datetime` AS date)pickup_datetime
# MAGIC   , CAST(f.` fare_amount` AS float) AS fare_amount
# MAGIC   , CAST(f.` surcharge` AS float) AS surcharge
# MAGIC   , CAST(f.` mta_tax` AS float) AS mta_tax
# MAGIC   , CAST(f.` tolls_amount` AS float) AS tolls_amount
# MAGIC   , CAST(f.` total_amount` AS float) AS total_amount
# MAGIC   , CAST(f.` tip_amount` AS float) AS tip_amount
# MAGIC   , CASE WHEN (f.` tip_amount` > 0) THEN 1 ELSE 0 END AS tipped
# MAGIC FROM vwTripFare as f
# MAGIC """).createOrReplaceTempView("vwTripFareCleansed")

# COMMAND ----------

spark.sql("""
SELECT 
  t.medallion
  ,t.` hack_license` AS hack_license
  ,CASE t.` vendor_id` WHEN 'CMT' THEN 1 ELSE 2 END AS vendor_id 
  ,t.` rate_code` rate_code
  ,t.` store_and_fwd_flag` AS store_and_fwd_flag
  ,CAST(t.` pickup_datetime` AS date)pickup_datetime
  ,CAST(t.` dropoff_datetime` AS date)dropoff_datetime
  ,dayofweek(CAST(t.` pickup_datetime` AS date)) PickUpWeekDay
  ,dayofweek(CAST(t.` dropoff_datetime` AS date)) DropOffWeekDay
  ,CAST(t.` passenger_count` AS int)passenger_count
  ,CAST(t.` trip_time_in_secs` AS int)trip_time_in_secs
  ,CAST(t.` trip_distance` AS float)trip_distance 
  ,CAST(t.` pickup_latitude` AS float)pickup_latitude
  ,CAST(t.` pickup_longitude` AS float) pickup_longitude
  ,CAST(t.` dropoff_latitude` AS float) dropoff_latitude
  ,CAST(t.` dropoff_longitude` AS float) dropoff_longitude
FROM vwTripData as t
""").createOrReplaceTempView("vwTripDataCleansed")

# COMMAND ----------

# MAGIC %md
# MAGIC 
# MAGIC #### Merge Trip data

# COMMAND ----------

# MAGIC %sql
# MAGIC 
# MAGIC SELECT * FROM vwTripFareCleansed LIMIT 10

# COMMAND ----------

# MAGIC %scala
# MAGIC val dfMerged = spark.sql("""
# MAGIC SELECT
# MAGIC   vendor_id
# MAGIC   ,PickUpWeekDay
# MAGIC   ,DropOffWeekDay
# MAGIC   ,passenger_count
# MAGIC   ,trip_time_in_secs
# MAGIC   ,trip_distance
# MAGIC  -- ,DirectDistance
# MAGIC   ,payment_type
# MAGIC   ,fare_amount
# MAGIC   ,surcharge
# MAGIC   ,mta_tax
# MAGIC   ,total_amount
# MAGIC   ,tipped
# MAGIC FROM vwTripDataCleansed t
# MAGIC JOIN vwTripFareCleansed f
# MAGIC ON t.medallion = f.medallion
# MAGIC AND   t.hack_license = f.hack_license
# MAGIC AND   t.pickup_datetime = f.pickup_datetime
# MAGIC AND   pickup_longitude != '0' AND dropoff_longitude != '0'
# MAGIC """)
# MAGIC 
# MAGIC dfMerged.createOrReplaceTempView("vwMerged")
# MAGIC dfMerged.write.format("parquet").save("dbfs:/mnt/nyctaxi/merged_aml_dbr")

# COMMAND ----------

# MAGIC %md
# MAGIC 
# MAGIC ## Create Azure Machine LEarning Workspace

# COMMAND ----------

pip install azureml-sdk

# COMMAND ----------

from azureml.core.experiment import Experiment
from azureml.core.workspace import Workspace
from azureml.train.automl.run import AutoMLRun
from sklearn.metrics import mean_squared_error
from azureml.core.compute import ComputeTarget, DatabricksCompute
from azureml.core.compute_target import ComputeTargetException
import math
from pyspark.sql.window import Window
from azureml.core.webservice import AciWebservice
from azureml.core.model import InferenceConfig
from azureml.core.model import Model
from azureml.core.webservice import Webservice
from azureml.core.conda_dependencies import CondaDependencies
from azureml.core.environment import Environment
from azureml.core.datastore import Datastore

# COMMAND ----------

ws = Workspace.get(name="mlws-poc-eastus",
               subscription_id='7e48a1e8-8d3e-4e00-8bc0-098c43f5ace7',
               resource_group='rg-machinelearning-eastus')

experiment_name = 'automl-nyctaxi-classification-dbr'
experiment=Experiment(ws, experiment_name)

# COMMAND ----------

# MAGIC %md
# MAGIC 
# MAGIC #### Create DBR Compute

# COMMAND ----------

db_compute_name = "dbr-compute-nyc"
db_resource_group = "rg-smartbox-aml-eastus-poc"
db_workspace_name = "dbr-smartbox-poc"
db_access_token = "dapid1f5aad874ac2b5a6696c08b15a996fd"
try:
    databricks_compute = ComputeTarget(workspace=ws, name=db_compute_name)
    print('Compute target {} already exists'.format(db_compute_name))
except ComputeTargetException:
    print('Compute not found, will use below parameters to attach new one')
    print('db_compute_name {}'.format(db_compute_name))
    print('db_resource_group {}'.format(db_resource_group))
    print('db_workspace_name {}'.format(db_workspace_name))
    print('db_access_token {}'.format(db_access_token))
 
    config = DatabricksCompute.attach_configuration(
        resource_group = db_resource_group,
        workspace_name = db_workspace_name,
        access_token= db_access_token)
    databricks_compute=ComputeTarget.attach(ws, db_compute_name, config)
    databricks_compute.wait_for_completion(True)

# COMMAND ----------

# MAGIC %md
# MAGIC 
# MAGIC #### Use databricks step 

# COMMAND ----------

ws.datastores

# COMMAND ----------

# MAGIC %md
# MAGIC 
# MAGIC #### Register ADLS as datastore

# COMMAND ----------

adlsgen2_datastore_name = 'dsadlsgen2'

subscription_id="7e48a1e8-8d3e-4e00-8bc0-098c43f5ace7"
resource_group="ADL_RESOURCE_GROUP", "rgSampleData"

account_name="sasampledata"
tenant_id="72f988bf-86f1-41af-91ab-2d7cd011db47"
client_id="2a81532b-016b-4c0e-aa43-bd9b97fbdaba"
client_secret="qlcF52cl2bo0[Nmo@-KuuVlNF[L9Ucs/"

adlsgen2_datastore = Datastore.register_azure_data_lake_gen2(workspace=ws,
                                                             datastore_name=adlsgen2_datastore_name,
                                                             account_name=account_name, # ADLS Gen2 account name
                                                             filesystem='nyctaxi', # ADLS Gen2 filesystem
                                                             tenant_id=tenant_id, # tenant id of service principal
                                                             client_id=client_id, # client id of service principal
                                                             client_secret=client_secret) # the secret of service principal

# COMMAND ----------

blob_datastore_name = 'dsblob'

subscription_id="7e48a1e8-8d3e-4e00-8bc0-098c43f5ace7"
resource_group="ADL_RESOURCE_GROUP", "rgSampleData"

account_name="sasampledata"
tenant_id="72f988bf-86f1-41af-91ab-2d7cd011db47"
client_id="2a81532b-016b-4c0e-aa43-bd9b97fbdaba"
client_secret="qlcF52cl2bo0[Nmo@-KuuVlNF[L9Ucs/"
account_key = "p5+k7W6bv9OIrCKpOA+p2Lbu8rrm+6D9eb5Fyv3hqO8j1GqmsYupeztdeaefzG7wScuugVbtGPrJn5BBZCqRsg=="
adlsgen2_datastore = Datastore.register_azure_blob_container(workspace=ws,
                                                             datastore_name=blob_datastore_name,
                                                             account_name=account_name, # ADLS Gen2 account name
                                                             container_name ='nyctaxi', # ADLS Gen2 filesystem
                                                             account_key = account_key)

# COMMAND ----------

print(dsNYCTaxi.datastore_type)

# COMMAND ----------

from azureml.pipeline.core import PipelineParameter,Pipeline, PipelineData
from azureml.data.data_reference import DataReference

# Use the default blob storage
dsNYCTaxi = Datastore.get(ws, "dsblob")
print('Datastore {} will be used'.format(dsNYCTaxi.name))

#pipeline_param = PipelineParameter(name="my_pipeline_param", default_value="pipeline_param1")


datasetFilePath = DataReference(datastore=dsNYCTaxi, path_on_datastore="/merged_aml_dbr",
                                     data_reference_name="datasetFilePath")

output = PipelineData("output", datastore=dsNYCTaxi)

# COMMAND ----------

from azureml.pipeline.steps import DatabricksStep
from azureml.core.databricks import PyPiLibrary

notebook_path="/Users/sitandon@microsoft.com/ModelTraining" # Databricks notebook path

dbNbStep = DatabricksStep(
    name="DBNotebookInWS",
    inputs=[datasetFilePath],
    outputs=[output],
    num_workers=1,
    notebook_path=notebook_path,
    run_name='DB_Notebook_demo',
    compute_target=databricks_compute,
    allow_reuse=True,
    spark_version="7.2.x-scala2.12",
    pypi_libraries=[PyPiLibrary(package = 'scikit-learn')
                    ,PyPiLibrary(package = 'azureml-sdk')
                    ,PyPiLibrary(package = 'lightgbm')
                    ,PyPiLibrary(package = 'pandas')],
    node_type = "Standard_D13_v2"
)

steps = [dbNbStep]
pipeline = Pipeline(workspace=ws, steps=steps)
pipeline_run = Experiment(ws, 'DB_Notebook_demo').submit(pipeline)
pipeline_run.wait_for_completion()

# COMMAND ----------

from azureml.widgets import RunDetails
RunDetails(pipeline_run).show()