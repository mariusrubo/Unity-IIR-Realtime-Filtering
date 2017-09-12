using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterData : MonoBehaviour {

    IIRFilter Notch50;
    IIRFilter Notch100;

    float input;
    float input_filtered1;
    float input_filtered2;

    // Use this for initialization
    void Start () {
        Notch50 = new IIRFilter(250, 50, 5); // sample rate 250Hz, Notch at 50Hz with bandwidth of 5
        Notch100 = new IIRFilter(250, 100, 5);
    }
	
	// Update is called once per frame
	void Update () {

        // input = SomeDataStream; // continuously stream your data in here. Need not be in Update(), as this is limited to 60Hz. Streaming data into Unity is not part of this script. 
        input_filtered1 = filter(input, Notch50);
        input_filtered2 = filter(input_filtered1, Notch100);
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

        // two parameters indicates a 2nd order Butterworth low-pass filter
        // equation obtained here: https://www.codeproject.com/Tips/1092012/A-Butterworth-Filter-in-Csharp
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

        // for Butterworth high-pass filters or other IIR filters, you need to insert parameters manually.
        // Easy way to find these parameters is using R's "signal" package butter function, and convert parameters like this: 
        // a0 = $b[0]; a1 = $b[1]; a2 = $b[2]; b1 = -$a[1]; b2 = -$a[2]
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
