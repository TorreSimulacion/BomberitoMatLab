using MathWorks.MATLAB.NET.Arrays;
using OT.Ternium.Math;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace OT.Ternium.Unity.MathModels
{
    public class FireController
    {
        #region Fields
        private bool _stop;
        private int _cols;
        private int _rows;
        private int _sizeX;
        private int _sizeY;
        private float[,] _temperature;
        private float[,] _fuel;
        private MWArray _mwaTemperature;
        private MWArray _mwaFuel;
        private ICustomLog _log;

        private float _k;
        private float _Tig;
        private float _Cs;
        private float _ST;
        private float _d;
        private float _B;
        private float _Arr;
        private int[,] _focus;

        #endregion

        #region Properties
        public float Tmax { get; set; }
        public float Sfin { get; set; }

        private float[,] _fuelUpdated = null;

        public float[,] Fuel
        {
            get
            {
                if (_fuelUpdated != null) return _fuelUpdated;
                return _fuel;
            }
            set
            {
                _fuelUpdated = value;
            }
        }
        private float[,] _temperatureUpdated = null;

        public float[,] Temperature
        {
            get
            {
                if (_temperatureUpdated != null) return _temperatureUpdated;
                return _temperature;
            }
            set
            {
                _temperatureUpdated = value;                
            }
        }
        public int NumFocus { get; set; }
        public float Alpha { get; set; }
        public StatusType Status { get; private set; }
        public long LastExecutionElapsed { get; private set; }
        #endregion

        #region Constructors
        public FireController(int sizeX, int sizeY, float tMax, float sFin, int numFocus, int[,] focus, float alpha, float k, float Tig, float Cs, float ST, int d, float B, float Arr)
        {
            Tmax = tMax;
            NumFocus = numFocus;
            Alpha = alpha;
            Sfin = sFin;
            _sizeX = sizeX;
            _sizeY = sizeY;
            _cols = sizeX / d;
            _rows = sizeY / d;
            _stop = true;
            _temperature = new float[_rows, _cols];
            _fuel = new float[_rows, _cols];
            _log = new DebugLog();
            Status = StatusType.Stoped;

            _k = k;
            _Tig = Tig;
            _Cs = Cs;
            _ST = ST;
            _d = d;
            _B = B;
            _Arr = Arr;
            _focus = focus;
        }
        #endregion

        #region Public Methods
        public void Start()
        {
            if (_stop)
            {
                InternalDebug("Started");
                _stop = false;
                ThreadPool.QueueUserWorkItem(WorkerThread);
            }
        }

        public void Stop()
        {
            _stop = true;
            InternalDebug("Stoped");
        }

        private void UpdateMWArray(float[,] array, MWArray mwArray)
        {
            int maxI = array.GetLength(0);
            int maxJ = array.GetLength(1);

            for (int i = 0; i < maxI; i++)
            {
                for (int j = 0; j < maxJ; j++)
                {
                    mwArray[j + 1, i + 1] = array[i, j];
                }
            }
        }

        private void UpdateMWArray(int[,] array, MWArray mwArray)
        {
            int maxI = array.GetLength(0);
            int maxJ = array.GetLength(1);

            for (int i = 0; i < maxI; i++)
            {
                for (int j = 0; j < maxJ; j++)
                {
                    mwArray[i + 1, j + 1] = array[i, j];
                }
            }
        }

        public void SetLog(ICustomLog log)
        {
            _log = log;
        }
        #endregion

        #region Private Methods
        private void WorkerThread(Object stateInfo)
        {
            Stopwatch sw;
            InternalDebug("WorkerThread started");
            Status = StatusType.Initializing;
            FireModel fireModel = null;

            try
            {
                sw = Stopwatch.StartNew();
                fireModel = new FireModel();
                sw.Stop();
                InternalDebug("FireModel constructor", sw.ElapsedMilliseconds);

                Status = StatusType.Initialized;
                
                MWNumericArray n = new MWNumericArray(_sizeX);
                MWNumericArray m = new MWNumericArray(_sizeY);
                MWNumericArray tMax = new MWNumericArray(Tmax);
                MWNumericArray sFin = new MWNumericArray(Sfin);
                MWNumericArray numFocus = new MWNumericArray(NumFocus);
                MWNumericArray alpha = new MWNumericArray(Alpha);

                MWNumericArray k = new MWNumericArray(_k);
                MWNumericArray Tig = new MWNumericArray(_Tig);
                MWNumericArray Cs = new MWNumericArray(_Cs);
                MWNumericArray ST = new MWNumericArray(_ST);
                MWNumericArray d = new MWNumericArray(_d);
                MWNumericArray B = new MWNumericArray(_B);
                MWNumericArray Arr = new MWNumericArray(_Arr);

                sw = Stopwatch.StartNew();
                MWArray initialMatrix = fireModel.matriz(n, m, d);
                sw.Stop();
                InternalDebug("matriz", sw.ElapsedMilliseconds);

                sw = Stopwatch.StartNew();
                _mwaFuel = fireModel.combustible(initialMatrix);
                sw.Stop();
                InternalDebug("combustible", sw.ElapsedMilliseconds);

                sw = Stopwatch.StartNew();
                MWArray mwaFocus;
                
                if (_focus != null)
                {
                    _mwaTemperature = initialMatrix;
                    mwaFocus = new MWNumericArray(_focus.GetLength(0), _focus.GetLength(1));
                    UpdateMWArray(_focus, mwaFocus);
                }
                else
                {
                    MWArray[] focus = fireModel.focos(2, initialMatrix, numFocus);// index 0 = temperatura con focos, index 1 = indices de focus
                    _mwaTemperature = focus[0];
                    mwaFocus = focus[1];
                }

                sw.Stop();
                InternalDebug("focos", sw.ElapsedMilliseconds);

                UpdateMatrixFromMWArray(_temperature, _mwaTemperature);
                UpdateMatrixFromMWArray(_fuel, _mwaFuel);

                while (!_stop)
                {
                    if (Status != StatusType.Executing)
                        Status = StatusType.Executing;

                    sw = Stopwatch.StartNew();


                    if(_fuelUpdated != null)
                    {
                        UpdateMWArray(_fuelUpdated, _mwaFuel);
                        _fuelUpdated = null;
                    }

                    if (_temperatureUpdated != null)
                    {
                        UpdateMWArray(_temperatureUpdated, _mwaTemperature);
                        _temperatureUpdated = null;
                    }

                    try
                    {
                        MWArray[] fire = fireModel.fire(2, _mwaTemperature, _mwaFuel, mwaFocus, tMax, sFin, alpha, k, Tig, Cs, ST, d, B, Arr);// index 0 = temperature, index 1 = fuel
                        _mwaTemperature = fire[0];
                        _mwaFuel = fire[1];

                        UpdateMatrixFromMWArray(_temperature, _mwaTemperature);
                        UpdateMatrixFromMWArray(_fuel, _mwaFuel);
                    }
                    catch (Exception ex)
                    {
                        InternalDebug(string.Format("fire exception: {0}", ex.Message));
                    }

                    sw.Stop();

                    LastExecutionElapsed = sw.ElapsedMilliseconds;
                    int sleep = sw.ElapsedMilliseconds > (int)(1000 * _k) ? 0 : ((int)(1000 * _k) - (int)sw.ElapsedMilliseconds);
                    InternalDebug(string.Format("Fire executed. Sleep: {0}", sleep), sw.ElapsedMilliseconds);
                    

                    Thread.Sleep(sleep);
                }
            }
            catch (Exception ex)
            {
                InternalDebug(ex.Message);
                InternalDebug(ex.StackTrace);
            }
            finally
            {
                if (fireModel != null)
                {
                    fireModel.WaitForFiguresToDie();
                    fireModel.Dispose();

                    InternalDebug("FireModel disposed");
                }
            }

            InternalDebug("WorkerThread finished");
            Status = StatusType.Stoped;
        }

        private void UpdateMatrixFromMWArray(float[,] matrix, MWArray mwarray)
        {
            double[,] a = (double[,])mwarray.ToArray();

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    matrix[i, j] = (float)a[j, i];
                }
            }
        }

        private void InternalDebug(string message)
        {
            _log.WriteLine(message);
        }

        private void InternalDebug(string message, long elapsedMilliseconds)
        {
            InternalDebug(string.Format("{0}, Elapsed: {1}", message, elapsedMilliseconds));
        }
        #endregion
    }
}
