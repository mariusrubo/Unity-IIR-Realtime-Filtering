# Unity-IIR-Realtime-Filtering
Apply 2nd order Butterworth Low-pass and High-pass filters or Notch filters or to data as they arrive

# Purpose
Physiological data typically need to be filtered before they can be further processed. For the purpose of data analysis, this is best done after data collection (e.g. using the 'signal' package in Python, Matlab or R). When data should be filtered in real-time, one can stream into Unity from fully-featured external software (such as BCI2000), but the setup is a little complex. This project instead allows to use Butterworth filters directly in Unity using C#. This may be useful for biofeedback applications using individual data streams (from ECG, EMG, EGG etc.), but also for filtering other things such as user input or character movement. For details see the comments in FilterData.cs. The filtering process follows that described in the Signal package (https://cran.r-project.org/web/packages/signal/signal.pdf). 

# Further Reading
* Anyone interested in digital signal filtering may find this free online book useful: http://dspguide.com/

# License
These scripts run under the GPLv3 license.
