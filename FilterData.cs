// Written by Marius Rubo

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterData : MonoBehaviour {

    IIRFilter Low26;
    IIRFilter High1;

    float input;
    float input_filtered1;
    float input_filtered2;

    void Start () {
	Low26 = new IIRFilter(500, 26); // 2nd order low pass butterworth 26hz at 500Hz sampling rate
	High1 = new IIRFilter(0.991153595101663f, -1.98230719020333f, 0.991153595101663f, 1.98222892979253f, -0.982385450614125f); // 2nd order high pass butterworth 1Hz at 500 Hz sampling rate
    }
	

   void Update () {

        // input = SomeDataStream; // continuously stream your data in here. Need not be in Update(), as this is limited to 60Hz. Streaming data into Unity is not part of this script. 
        input_filtered1 = filter(input, Low26);
        input_filtered2 = filter(input_filtered1, High1);
    }

    public class IIRFilter
    {
        public float a0;
        public float a1;
        public float a2;
        public float b1;
        public float b2;

        public float x1;
        public float x2;
        public float y1;
        public float y2;

	// two parameters indicate a 2nd order Butterworth low-pass filter
        // equation obtained here: https://www.codeproject.com/Tips/1092012/A-Butterworth-Filter-in-Csharp
	// note that you can also use the five-parameter-solution described below. This is just for convenience. 
        public IIRFilter(float samplingrate, float frequency)
        {
            const float pi = 3.14159265358979f;
            float wc = Mathf.Tan(frequency * pi / samplingrate);
            float k1 = 1.414213562f * wc;
            float k2 = wc * wc;
            a0 = k2 / (1 + k1 + k2);
            a1 = 2 * a0;
            a2 = a0;
            float k3 = a1 / k2;
            b1 = -2 * a0 + k3;
            b2 = 1 - (2 * a0) - k3;

            x1 = x2 = y1 = y2 = 0;
        }
	    
        // three parameters indicates a notch filter
        // equation obtained here http://dspguide.com/ch19/3.htm
        public IIRFilter(float samplingrate, float frequency, float Bandwidth)
        {
            float Pi = 3.141592f;
            float BW = Bandwidth / samplingrate;
            float f = frequency / samplingrate;
            float R = 1 - 3 * BW;
            float K = (1 - 2 * R * Mathf.Cos(2 * Pi * f) + R * R) / (2 - 2 * Mathf.Cos(2 * Pi * f));
            a0 = K;
            a1 = -2 * K * Mathf.Cos(2 * Pi * f);
            a2 = K;
            b1 = 2 * R * Mathf.Cos(2 * Pi * f);
            b2 = -R * R;

            x1 = x2 = y1 = y2 = 0;
        }

	/*
	Five parameters indicate a generic filter. 
	This is necessary for Butterworth high-pass filters or other IIR filters (because I could not implement the process of obtaining these parameters in here).  
        Easy way to find these parameters is using R's "signal" package butter function, and convert parameters like this: 
	
	samplingRate <- 500 # in Hz
	cutoff <- 1 # in Hz
	
	order <- 2 # 
	nyquist <- samplingRate/2
	W <- cutoff/nyquist
	bf <- signal::butter(order, W, type = "high")
        a0<- bf$b[1] / bf$a[1]
        a1<- bf$b[2] / bf$a[1]
        a2<- bf$b[3] / bf$a[1]
        b1<- -bf$a[2] / bf$a[1]
        b2<- -bf$a[3] / bf$a[1]
	paste0("new IIRFilter(",a0, "f, " ,a1, "f, ", a2, "f, ", b1, "f, ", b2, "f);") 
        */
        public IIRFilter(float a0in, float a1in, float a2in, float b1in, float b2in)
        {
            a0 = a0in;
            a1 = a1in;
            a2 = a2in;
            b1 = b1in;
            b2 = b2in;

            x1 = x2 = y1 = y2 = 0;
        }
    }

    // filter data. Each IIRFilter stores two data points of filtered and unfiltered data. Therefore, filtering should be continuous and not be switched on and off. 
    // Furthermore, each IIRFilter may only process one data stream. If you intend to filter two data streams with the same kind of filter, you need to initialize 
    // two IIRFilters accordingly (e.g. "Notch50_1" and "Notch50_2"), each filtering only one data stream. 
    public float filter(float x0, IIRFilter iirfilter)
    {
        float y = iirfilter.a0 * x0 + iirfilter.a1 * iirfilter.x1 + iirfilter.a2 * iirfilter.x2 + iirfilter.b1 * iirfilter.y1 + iirfilter.b2 * iirfilter.y2;

        iirfilter.x2 = iirfilter.x1;
        iirfilter.x1 = x0;
        iirfilter.y2 = iirfilter.y1;
        iirfilter.y1 = y;

        return y;
    }
}
