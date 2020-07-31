## Problem Definition

The insurance company identified that around 25% of all the cases that all the claims received, are fraudulent. The company does not typically identify those cases up until after the claim has been closed.

The insurance wants to find a way to identify those fraudulent cases beforehand. We are going use Data Science in order to help in this identification.

## Performance Metrics

They want to identify the fraudulent cases while avoiding miss-classification of non-fraudulent cases. However; we have an imbalanced number cases for **frauds (25%)** vs. **non frauds(75%)**, thus **Accuracy** might not be ideal metric. Thus we are going to track the following two cases:
- **Accuracy**
- **F1_score**:  harmonic mean between **Precision** and **Recall**

More information on the metrics can be found in the following [link](https://en.wikipedia.org/wiki/Evaluation_of_binary_classifiers)