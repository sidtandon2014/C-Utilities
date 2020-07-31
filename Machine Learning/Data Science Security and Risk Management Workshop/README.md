# Data Science Security & Risk Management 2-days Workshop
Welcome!

## Do the Demos

If you are here to reproduce a demo in the comfort of your home/office, go to the [notebooks](https://dsworkshopwork9135752343.blob.core.windows.net/additional-docs/ds_workshop.zip?st=2020-06-28T22%3A57%3A49Z&se=2020-07-14T22%3A57%3A00Z&sp=rl&sv=2018-03-28&sr=b&sig=KY0bmApIkzLHtx%2Fv3Y5nueUgt8ZKsGzKYvS09isxWTs%3D). At the end of this document you will find deployment instructions, to create the environment you need, and a tutorial to do the demo step by step.


## About this workshop:

Data Science is a very expansive field whose techniques and frameworks are constantly been updated. Because of this, typically it is hard to determine what approaches are right to solve for our specific business needs. 

To attend to this need, we created this **2-day workshop**, where we are going to walk you through the whole Data Science Process using the recommended best practices to develop Machine Learning models while protecting the data and controlling the risks associated to them. This content is intended to be delivered through MS Teams and we assume that the attendants will only have only a minimum level of proficiency in Azure and Data Science. 

As for the agenda, this workshop is going to be 2 days long:

![day_1](./Misc/day_1.PNG)

![day_2](./Misc/day_2_mid.PNG)

Additionally, we are going make available **video recordings** that will serve as baselines for the modules. They will contain background information and deep dives on modules.


## Who is for?
The Data Science process, by nature is a very iterative and collaborative process. This been said, we recommend companies to bring multi-functional teams so that they can simulate the collaboration in real life Machine Learning projects. 
- Product Manager / Business Decision-Maker.
- DevOps team / IT Admin
- Data Scientists
- Data Engineers
- Software developers

Additionally, although we recommend attendants to have experience coding, as Python is going to be the main language used throughout the workshop, the content is going to be developed so that anybody can understand best practices and recommendations on a high level.

## Modules

Here are all the modules available that are part of this program:


### [**Module 1**: Avoiding Risks Introduced Before Model Creation](./Module_1-Before_Model_Creation)

In this module, we are going to talk about all the validations that we recommend doing before the creation on Machine Learning models. As Machine Learning models can have both **Intrinsic** *(data and assumptions related)* and **Operational** *(process and infrastructure related)* risks, we are going to talk about the following points:
 1. [**Topic 1**: Data Science Lifecycle](./Module_1-Before_Model_Creation/Topic_1-DS_lifecycle/README.md): How do you coordinate workflows enabling data science teams to work together and make sure that everything becomes a shared asset.
 2. [**Topic 2**: Data Ingestion](./Module_1-Before_Model_Creation/Topic_2-Data_ingestion/): Avoiding Risks Associated with Data Acquisition.
 3. [**Topic 3**: Model Objective Validation](./Module_1-Before_Model_Creation/Topic_3-Model_objective_validation/): Avoiding risks associated with design objectives of Machine Learning models and quality of input data.

### [**Module 2**: Avoiding Risks Introduced During and After Model Creation](./Module_2-During_and_After_Model_Creation)

Now that we went trough the process and recommendations for avoiding data-related risks. In this module, we are to give recommendations on avoiding risks associated with the model creation and deployment processes. All having into account that Machine Learning models can have both **Intrinsic** *(data and assumptions related)* and **Operational** *(process and infrastructure related)*. In this module we are going to talk about the following topics:
1. [**Topic 4**: Model Creation](./Module_2-During_and_After_Model_Creation/Topic_4-Model_Creation/README.md): Avoiding Risks Associated with Model Creation & Selection. Tabular Data. **(includes lab).**
2. [**Topic 5**: Model Deployment and Pipeline](./Module_2-During_and_After_Model_Creation/Topic_5-Model_Deployment_and_Pipelines/README.md): Model Deployment and Automation (MLOps) **(includes lab).**

## Prerequisites

- [An understanding of Git](https://docs.microsoft.com/en-us/learn/paths/intro-to-vc-git/)
- [An understanding of Python](https://aka.ms/pythonbeginnerseries)
- [An understanding of Azure Machine Learning](https://docs.microsoft.com/en-us/learn/modules/intro-to-azure-machine-learning-service/)

### Setup steps

#### Day 1:

1. Create **Azure Machine Learning Workspace**
2. Create VM of size: **STANDARD_D4_v2**
3. Launch **Jupyter Notebooks** from the VM, in the **Compute** section. and Launch a new **Terminal**.
4. Download files in the following link: [Data Science Workshop](https://dsworkshopwork9135752343.blob.core.windows.net/additional-docs/ds_workshop.zip?st=2020-06-29T11%3A54%3A03Z&se=2020-07-30T11%3A54%3A00Z&sp=rl&sv=2018-03-28&sr=b&sig=OWIRT7FjAllyiIbI9H3Ah%2BAuy3GdMe7WhYWun%2FOhsCs%3D)
5. Unzip file: `unzip ds_workshop.zip`
6. If we come back to jupyter notebooks, you will see that a new folder has been created.
7. Come back to the terminal, and lets install some things:
    - `pip install -U pandas`
    - `pip install pandas_profiling`
    - `pip install interpret`
    - `pip install cufflinks`
    - `pip uninstall enum34`
    - `pip install hdbscan`
    - `pip install enum34`
8. Open notebook in **ds_workshop/module_1/model_objective_validation.ipynb**
9. Replace value for **SUBSCRIPTION_ID, RESOURCE_GROUP and WORKSPACENAME**

### Setup steps

#### Day 2:

1. Launch **Jupyter Notebooks** from the VM, in the **Compute** section.
2. Open terminal and run the following code:
    - `pip install pandas==0.23`
    - `pip install boruta`
3. Open notebook in **ds_workshop/module_2/model_creation.ipynb**

## Next steps

As the goal of this course is to help get you up to speed on Python so you can work through a quick start, the next step after completing the videos is to follow a tutorial! Here's a few of our favorites:

- [Predict flight delays by creating a machine learning model in Python](https://docs.microsoft.com/learn/modules/predict-flight-delays-with-python?WT.mc_id=python-c9-niner)
- [Train a machine learning model with Azure Machine Learning](https://docs.microsoft.com/learn/modules/train-local-model-with-azure-mls?WT.mc_id=python-c9-niner)
- [Analyze climate data](https://docs.microsoft.com/learn/modules/analyze-climate-data-with-azure-notebooks?WT.mc_id=python-c9-niner)
- [Use unsupervised learning to analyze unlabeled data](https://docs.microsoft.com/learn/modules/introduction-to-unsupervised-learning?WT.mc_id=python-c9-niner)


## Trained Presenters

Jose Medina

[![Learning Path](./Misc/pp_smallest.PNG)](https://www.linkedin.com/in/joselom23/)
