# Dataset Questionnaire for Model Deployment

## Overview

In the previous topic, we focused on evaluating and reducing some of the **Intrinsic Risks** of Machine Learning Models. 
However, in this final topic, we are going to be focusing on the **Operational Risks** associated with Machine Learning models. This are risks associated with the models after they are deployed. This included the re-training and post deployment maintenance.

 
-------------------------------

## Questions:

### 1. Fitness of Model:

- **Are you using the model on the right data?** One example can be that you work for a sales company and you create a model from one channel, in most cases, I would not be able to apply the same model to other channel. Using models in datasets that have a different distribution from the Training set, can cause unpredictable results.
- **Do you evaluate how your models performance over time?** Do you have a process in place to track all how consistent your models are over the time?


### 2. Auditability:

- **How are you making sure that you model can be auditable?** Can I explain why the model is taking such decisions?

### 3. Security:
                                                                                             
- **Who can access to my model/application?** How are you controlling the access to your development environment?
- **Which artifacts of my models are exposed and where are they exposed?**



-------------------------------

## Demo:

You can find this exercise for the Demo dataset in the following link: [Notebook]()

-------------------------------

## Supervisory Guidanve on Model Risk Management (FDIC):

Please go to the following documentation: [FDIC](https://www.fdic.gov/news/financial-institution-letters/2017/fil17022a.pdf)

-------------------------------

## More information:

We recommend reading the following paper: ***Co-Designing Checklists to Understand Organizational Challenges and Opportunities around Fairness in AI***. More information [here](http://www.jennwv.com/papers/checklists.pdf)