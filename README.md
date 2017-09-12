# Unity-IIR-Online-Filtering
Apply Notch filters or Butterworth Low-pass and High-pass filters to data as it arrives

# Purpose
Data from external hardware like OpenBCI, Bitalino, Arduino etc. typically need to be filtered before they can be processed. When you need to analyze data after it is collected (e.g. in a psychological or medical experiment), there exist excellent packages both for MATLAB and R. If your Unity software needs to respond dynamically to input, one option is to send it to MATLAB, process the data there, and send it back. This project, however, aims at performing simple filtering of data directly in C#. Specifically, 2nd order Notch and Butterworth filters, but also other IIR filters can be implemented and used on data as it is streamed into Unity. For details see the comments in FilterData.cs. 

# Example
The below image shows data that was streamed in from an OpenBCI. Nothing was being measured, to there should be a straight line. However, as you can see on the left part of the diagram, data is distorted by a 50 Hz electric hum. With a sample rate of 250 Hz, displaying data as points rather than lines makes it look like there are 5 different data streams in one. The middle part of the diagram shows the same kind of data, but with a 50Hz Notch filter as in the attached FilterData.cs file. In the right part of the diagram, data is furthermore filtered using a 100Hz Notch filter. As you can see, data resembles much more, but still not perfectly a straight line. This would be better achieved using the filtfilt-function, which can however not be applied in online filtering.

![alt tag](https://github.com/mariusrubo/Unity-IIR-Online-Filtering/blob/master/filterexample.jpeg)

# Further Reading
* Online filtering this way does not allow for zero-phase filtering as achieved in the well-known "filtfilt" function. For an introduction into this topic I recommend this video: https://www.youtube.com/watch?v=ue4ba_wXV6A
* Anyone interested in digital signal filtering may find this free online book useful: http://dspguide.com/
* For receiving data in Unity through LSL, see this excellent repository: https://github.com/xfleckx/LSL4Unity

# License
These scripts run under the GPLv3 license. See the comments inside the scripts for more details and ideas on how to adapt them for your own projects.
