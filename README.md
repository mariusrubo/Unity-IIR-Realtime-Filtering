# Unity-IIR-Realtime-Filtering
Apply Notch filters or Butterworth Low-pass and High-pass filters to data as it arrives

# Purpose
Data from external hardware like OpenBCI, Bitalino, Arduino etc. typically need to be filtered before they can be processed. When you need to analyze data after it is collected (e.g. in a psychological or medical experiment), there exist excellent packages both for MATLAB and R. If your Unity software needs to respond dynamically to input, one option is to send data to MATLAB, perform signal processing there, and then send data to Unity. This project, however, aims at performing simple filtering of data directly in C#. Specifically, 2nd order Notch and Butterworth filters, but also other IIR filters can be implemented and used on data as it is streamed into Unity. For details see the comments in FilterData.cs. 

# Example
The below image shows ecg data that was streamed in from an OpenBCI. The upper part of the diagram shows the ecg data, while the lower part shows absolute differences for each data point to the previous data point. In the left third of the diagram, data is not filtered at all, and the 50Hz electric hum is clearly visible. In the middle part, a 50Hz Notch filter is applied. In the right part, a 100Hz Notch filter, a 2nd-order 1Hz Butterworth Highpass filter and a 2nd-order 50Hz Butterworth Lowpass filter are applied additionally. Note that the is ecg is displayed "the wrong way round", but absolute changes in voltage are not affected. Both the 50Hz Notch filter and the other filters lead to a substantial increase in signal-to-noise-ratio, which is most clearly visible on the change in voltage.
![alt tag](https://github.com/mariusrubo/Unity-IIR-Online-Filtering/blob/master/filterexample.jpeg)

# Further Reading
* Online filtering this way does not allow for zero-phase filtering as achieved in the well-known "filtfilt" function. For an introduction into this topic I recommend this video: https://www.youtube.com/watch?v=ue4ba_wXV6A
* Anyone interested in digital signal filtering may find this free online book useful: http://dspguide.com/
* For receiving data in Unity through LSL, see this excellent repository: https://github.com/xfleckx/LSL4Unity

# License
These scripts run under the GPLv3 license.
