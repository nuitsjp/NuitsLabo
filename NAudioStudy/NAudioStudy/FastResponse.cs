namespace NAudioStudy;

using System;
using NAudio.Wave;

class FastResponse(double alpha = 0.125)
{
    private double _previousValue;

    public double Process(double value)
    {
        double output = alpha * value + (1 - alpha) * _previousValue;
        _previousValue = output;
        return output;
    }
}
