# Dataset Questionnaire for Data Ingestion Process

## Overview

As **Data** Scientists, often we generate or receive the datasets and immediately start working on solving our problem. However, by doing so, we might ignore some important aspects, that if not correctly documented and addressed, can introduce unnecessary risks to your processes. To avoid those risks we created a **Questionnaire for Datasets** to help with this documentation process. ***We recommend creating a new questionnaire for each dataset***.

Following you will find a series of questions that, even though in some cases might not be enough, can work as a baseline to help you with both reducing the risks associated with data sources and also to consistently **re-create or update** your datasets. These questions will complement the questions from the **Model Objective Validation phase**.

-------------------------------

## Questions:

### 1. Motivation:

- **For what purpose was the dataset created?** What processes and/or models are going to be *created and/or modified* by using this dataset?
- **What is the origin of the dataset?** *Is this a company dataset? was it provided by a third-party vendor? Is this an open source dataset?*
- **Who created this dataset?** Who is the author of this dataset?

### 2. Data Composition:

- **Has an analysis of the potential impact of the dataset and its use on data subjects been conducted? If so, where is the location of this analysis?**
- **Does this dataset include PII? If previous yes? What is the strategy secure this personal information?** Are you using Tokenization? Are you obfuscating the data, are you removing variables?
- **Is it possible to identify individuals?**  (i.e., one or more natural persons), either directly or indirectly (i.e., in combination with other data) from the dataset? Does the dataset contain linkable information?
- **If yes, what are those fields?** explicitly mention these fields.

### 3. Collection Process:

- **What is the source system of the dataset?** in here specify the name of API, OLAP or OLTP system. If the file is in the cloud or in an URL, blob storage, provide the **path** to find dataset.
- **What is the structure of data?**  What is the structure of the dataset? *Tabular? Non-Tabular? Graph**. More information on data types [here](https://en.wikipedia.org/wiki/Data_type)
- **What is the content of this dataset?** This refers to the type of data contained in each observation of this dataset. It can be more than one. Some examples are: * Text, Video, Images, Numbers, Dates**
- **What is the level of refinement of the data used** this refers to whether the data is *raw, preprocessed or mixed?*
- **Where is the source schema?** point to a location where this source schema can be found.
- **Where is the source repository?** since we recommend that data *(formatting)* pipelines should be reproducible from raw data. These data *(formatting)* pipelines should be stored in a version control repository. 
These pipelines do not necessarily  refer to pipelines for the model creation (i.e. Feature engineering), they refer to processing done before storing the data that will be used in your process and/or other processes.
- **What kind of information is contained in each observation?** Does each observation refers to a transaction? a sale? personal history? a claim policy? to the specific stock price of a company in a specific date?
- **Does the dataset relate to people?** In a lot of cases, datasets do not relate to specific people.
- **Are any individual fields or dataset subjected to any regulatory requirement?**
- **If previous yes? Please provide those fields** provide them in a list format: **["feature_a, feature_b"]**
- **Is the dataset a sample from a larger set?** In some cases, having the full dataset to start working in a project might not be practical and in some cases you do not have access to the full dataset.
- **If yes, what is the sampling strategy?** It can be deterministic (date cut-offs), probabilistic with specific sampling probabilities or any sampling strategy. Also, provide documentation of this sampling process.
- **If yes, do you have a plan to run the full data science process on the full dataset?** this can be yes or not.

### 4. Data Storage:

- **Where is the data stored?** Provide link to documentation about data location or link to data location.

### 5. Data Distribution:

- **How can the data be accessed?** Provide documentation on how to access data. *code, URL location, etc...*
- **Who has the rights to provide access to this dataset?** Provide contact of person that provide access to this dataset.
- **How access to this dataset can be granted?** Provide documentation on how to request access to this dataset. 
- **How are you are going to guarantee that all processes read the data correctly?** Provide documentation on how you are planning on doing this.

### 6. Data Maintenance:

- **Who will be supporting/hosting/maintaining the dataset?**
- **How can the owner/curator/manager of the dataset be contacted?** Please provide contact information.
- **How often will this dataset be updated? (i.e. adding new samples)?** The frequency can be ad-hoc, daily, weekly, monthly among others.
- **Will older versions of the dataset continue to be supported/hosted/maintained?** We recommend that always yes.
- **How are you going to document each version of the dataset?** What is the plan to create and persist different data versions?
- **If others want to extend/augment/build on/contribute to the dataset, is there a mechanism for them to do so?** An example can be by using a ***Issue*** in GitHub.

-------------------------------

## Demo:

You can find this exercise for the Demo dataset in the following link: [Dataset Questionnaire for Data Ingestion Process.](dataset_questionnaire_data_ingestion.xlsx)

-------------------------------

## Securing Data Ingestion, Collection and Storage Processes for Data Science in Azure:

Please go to the following documentation: [Documentation](./documentation_helper/1_2_data_gathering_and_injection_security.pdf)

-------------------------------

## More information:

We used the ***Datasheets for Datasets*** paper as inspiration to create this questionnaire. More information [here](https://arxiv.org/pdf/1803.09010.pdf)