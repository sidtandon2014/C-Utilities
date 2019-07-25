// Databricks notebook source
import org.apache.spark.sql.types.{StringType, StructType, _}
import org.apache.spark.eventhubs.{ ConnectionStringBuilder, EventHubsConf, EventPosition }
import org.apache.spark.sql.functions.{ explode, split }
import org.apache.spark.sql.types._
import org.apache.spark.sql.functions._
import org.apache.spark.sql.DataFrame
import org.apache.spark.streaming._
import org.apache.spark.sql.streaming.{
  GroupState,
  GroupStateTimeout,
  OutputMode
}
import java.sql.Timestamp
import com.google.gson.Gson
import com.google.gson.JsonObject
import com.google.gson.JsonParser
import argonaut.Argonaut._
import argonaut._

// COMMAND ----------

val schema = (new StructType()
              .add("user", StringType).add("timestamp",LongType).add("activity", StringType)
)

// COMMAND ----------

case class InputRow(user:String, timestamp:java.sql.Timestamp, activity:String)
case class UserState(user:String,
  var activity:String,
  var start:java.sql.Timestamp,
  var end:java.sql.Timestamp,
                    var timeIndex: Int)

// COMMAND ----------

def updateUserStateWithEvent(state:UserState, input:InputRow):UserState = {
// no timestamp, just ignore it  
if (Option(input.timestamp).isEmpty) {
    return state
  }
//does the activity match for the input row
if (state.activity == input.activity) {
    if (input.timestamp.after(state.end)) {
      state.end = input.timestamp
      state.timeIndex += 1
    }
    if (input.timestamp.before(state.start)) {
      state.start = input.timestamp
      state.timeIndex = 0
    }
  } else { 
   //some other activity
    if (input.timestamp.after(state.end)) {
      state.start = input.timestamp
      state.end = input.timestamp
      state.activity = input.activity
      state.timeIndex = 0
    }
  }
  //return the updated state
  state
}

def updateAcrossEvents(user:String,
    inputs: Iterator[InputRow],
     oldState: GroupState[UserState]):UserState = {
     var state:UserState = if (oldState.exists) oldState.get else UserState(user,
        "",
        new java.sql.Timestamp(6284160000000L),
        new java.sql.Timestamp(6284160L),
        -1
    )
  // we simply specify an old date that we can compare against and
  // immediately update based on the values in our data

  for (input <- inputs) {
    state = updateUserStateWithEvent(state, input)
    oldState.update(state)
  }
  state
}

// COMMAND ----------

val connectionString = ConnectionStringBuilder(s"Endpoint=sb://rawfilesehns.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=f0wIwiLXz4g3FXPNJLf9Vlv+g+cZpelewH4DsUGkdZM=")
  .setEventHubName("testeventhub")
  .build

val eventHubsConf = EventHubsConf(connectionString)
.setMaxEventsPerTrigger(5)
  //.setStartingPosition(EventPosition.fromEndOfStream)
  
val eventhubs = (spark.readStream
  .format("eventhubs")
  .options(eventHubsConf.toMap)
  .load()
                 )
def deserializeUserEvent(json: String): InputRow = {
  val jsonStringAsObject= new JsonParser().parse(json).getAsJsonObject
  val userEvent:InputRow = new Gson().fromJson(jsonStringAsObject, classOf[InputRow])
  return userEvent
  }

import org.apache.spark.sql.functions._

val messages = (eventhubs
  .selectExpr("cast (body as string) AS Content")
  .select(from_json($"Content",schema).alias("col1"))
  .withColumn("user",$"col1.user").withColumn("activity",$"col1.activity").withColumn("timestamp",expr("cast(col1.timestamp as timestamp)")).drop($"col1")
  .as[InputRow]
  //.map(row => deserializeUserEvent(row.getString(0)))
  .groupByKey(_.user)
  .mapGroupsWithState(GroupStateTimeout.NoTimeout)(updateAcrossEvents)
)
                
//val messages = (eventhubs.selectExpr("cast (body as string) AS Content").map(_.getString(0)))
                /*select(from_json($"Content",schema).alias("col1")).withColumn("id",$"col1.id")
.withColumn("name",$"col1.name").withColumn("value",$"col1.value").withColumn("currentTime",$"col1.currentTime")
.drop($"col1"))*/

//messages.as[UserEvent]
messages
.writeStream
//.queryName("events_per_window")
.outputMode("update")
.format("console")
//.option("checkpointLocation", "/mnt/chkpointloc")
.start().awaitTermination()

// COMMAND ----------

// MAGIC %sql
// MAGIC 
// MAGIC SELECT * FROM events_per_window WHERE user = 'user2' ORDER BY user
