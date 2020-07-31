# Dataset Questionnaire for Model Objective Validation

## Overview

Having a clear view of ***business objectives*** and defining achievable goals are important tasks to ensure that results will meet expectations from the business. Deeply understanding the content of your dataset is crucial when identifying future ***risks and limitations*** of your Machine Learning models and processes.

Following you will find a series of questions that, even though in some cases might not be enough, can work as a baseline to help you with both reducing the risks associated with ***unrealistic expectations*** and  with ***limitations of the dataset***. These questions will complement the questions from the **Data Validation phase**.

-------------------------------

## Questions:

### 1. Data Composition:

- **Does the dataset contain data that might be considered sensitive in any way?** (e.g., data that reveals racial or ethnic origins, sexual orientations, religious beliefs, political opinions or union memberships, or locations; financial or health data; biometric or genetic data)
- **Does the dataset identify any subpopulations? If yes, please identify those variables** This question can help in the identification of possible biases or under-representation of certain subpopulations. *Age, Sex, Gender and Race* are examples.
- **Do you have a data profile? If yes, please provide link**: Data profiling refers to the analysis of information for use in Data Science in order to clarify the structure, content, relationships, and derivation rules of the data [DLT](https://learning.oreilly.com/library/view/the-data-warehouse/9780470149775/chapter-137.html) . More information [here.](https://en.wikipedia.org/wiki/Data_profiling)
- **Is there a label or target associated with each instance? IF so, what are the name of the fields that contain those labels?** It might sound counterintuitive but not all datasets contain labels. In some cases you are not interested in predicting something, in some cases you need to generate your own labels from the data provided.
- **Do you have a data split strategy?** Here specify the data split strategy used when evaluating both the baseline model and the final models. More information on *Data Splitting* [here.](https://www.mff.cuni.cz/veda/konference/wds/proc/pdf10/WDS10_105_i1_Reitermanova.pdf)
- **Is the dataset self-contained, or does it link to or otherwise rely on external resource?** The answer here can be *additional websites, tweets, or other datasets*.

### 2. Data preprocessing or pre-formatting. (Before Feature Engineering):

- **Do you have the documentation preprocessing/cleaning/labeling of the data? If so, provide link to code**: This question is based on the pre-processing of the data before storage. This is independent from the data preprocessing during *Feature Engineering*.
- **Was the “raw” data saved in addition to the preprocessed/cleaned/labeled data?** (e.g., to support unanticipated future uses)? This only applies to changes in data to be stored.
- **Are there tasks for which the dataset should not be used?**: Please specify whether this data cannot be used in other processes. This can be because several reasons: regulations, data not being adequate for other processes, etc...

### 3. Data Use:

- **Could this dataset be used for other tasks and/or processes?**
- **Are there tasks for which the dataset should not be used?** 

### 4. Model Baseline:

- **Did you identify the set of metrics that will translate the business problem into Machine Learning?** This is very important; we need to have a way of measuring the improvement of the Machine Learning models.
- **Do you have a business explanation why you chose those metrics?** It is important to have performance metrics that translated your business problems into an optimization problem, so that each improvement in those metrics can be translated to actual improvements in your processes.
- **How did you define the baseline?**  Did you measure the performance of an existing ML model? Or are you using the performance of a current process based on heuristics or other methodology?
- **How are you evaluating possible outliers and extreme values in dependent and independent variables?** It is important to identify these cases, as they will help you in stress testing your Machine Learning models.
- **How are you evaluating the overall consistency labels (target) vs. independent variables? How are you identifying wrong labels or outputs?** it is important to identify possible outliers, you can do this by using an **anomaly detection model**.
- **Are you building any baseline models for this experiment?** If not available, we recommend creating a baseline model with a simple and fast algorithm, even before doing **Feature Engineering**, this will give you insights on what can be achievable with the current data.



-------------------------------

## Demo:

You can find this exercise for the Demo dataset in the following link: [Model Objective Questionnaire.](model_ov_questionnaire.xlsx)

-------------------------------

## Supervisory Guidance on Model Risk Management (FDIC):

Please go to the following documentation: [FDIC](https://www.fdic.gov/news/financial-institution-letters/2017/fil17022a.pdf)

-------------------------------

## More information:

We recommend reading the following paper: ***Co-Designing Checklists to Understand Organizational Challenges and Opportunities around Fairness in AI***. More information [here](http://www.jennwv.com/papers/checklists.pdf)