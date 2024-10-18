/*
* MATLAB Compiler: 6.3 (R2016b)
* Date: Thu Sep 05 17:57:44 2019
* Arguments: "-B" "macro_default" "-W" "dotnet:OT.Ternium.Math,FireModel,3.5,private"
* "-T" "link:lib" "-d" "D:\dev\Producto
* Cursos\PrevencionIncendio\Matlab.Model\fire\fire\for_testing" "-v"
* "class{FireModel:D:\dev\Producto
* Cursos\PrevencionIncendio\Matlab.Model\fire\combustible.m,D:\dev\Producto
* Cursos\PrevencionIncendio\Matlab.Model\fire\fire.m,D:\dev\Producto
* Cursos\PrevencionIncendio\Matlab.Model\fire\focos.m,D:\dev\Producto
* Cursos\PrevencionIncendio\Matlab.Model\fire\matriz.m}" 
*/
using System;
using System.Reflection;
using System.IO;
using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;

#if SHARED
[assembly: System.Reflection.AssemblyKeyFile(@"")]
#endif

namespace OT.Ternium.Math
{

  /// <summary>
  /// The FireModel class provides a CLS compliant, MWArray interface to the MATLAB
  /// functions contained in the files:
  /// <newpara></newpara>
  /// D:\dev\Producto Cursos\PrevencionIncendio\Matlab.Model\fire\combustible.m
  /// <newpara></newpara>
  /// D:\dev\Producto Cursos\PrevencionIncendio\Matlab.Model\fire\fire.m
  /// <newpara></newpara>
  /// D:\dev\Producto Cursos\PrevencionIncendio\Matlab.Model\fire\focos.m
  /// <newpara></newpara>
  /// D:\dev\Producto Cursos\PrevencionIncendio\Matlab.Model\fire\matriz.m
  /// </summary>
  /// <remarks>
  /// @Version 3.5
  /// </remarks>
  public class FireModel : IDisposable
  {
    #region Constructors

    /// <summary internal= "true">
    /// The static constructor instantiates and initializes the MATLAB Runtime instance.
    /// </summary>
    static FireModel()
    {
      if (MWMCR.MCRAppInitialized)
      {
        try
        {
          Assembly assembly= Assembly.GetExecutingAssembly();

          string ctfFilePath= assembly.Location;

          int lastDelimiter= ctfFilePath.LastIndexOf(@"\");

          ctfFilePath= ctfFilePath.Remove(lastDelimiter, (ctfFilePath.Length - lastDelimiter));

          string ctfFileName = "Math.ctf";

          Stream embeddedCtfStream = null;

          String[] resourceStrings = assembly.GetManifestResourceNames();

          foreach (String name in resourceStrings)
          {
            if (name.Contains(ctfFileName))
            {
              embeddedCtfStream = assembly.GetManifestResourceStream(name);
              break;
            }
          }
          mcr= new MWMCR("",
                         ctfFilePath, embeddedCtfStream, true);
        }
        catch(Exception ex)
        {
          ex_ = new Exception("MWArray assembly failed to be initialized", ex);
        }
      }
      else
      {
        ex_ = new ApplicationException("MWArray assembly could not be initialized");
      }
    }


    /// <summary>
    /// Constructs a new instance of the FireModel class.
    /// </summary>
    public FireModel()
    {
      if(ex_ != null)
      {
        throw ex_;
      }
    }


    #endregion Constructors

    #region Finalize

    /// <summary internal= "true">
    /// Class destructor called by the CLR garbage collector.
    /// </summary>
    ~FireModel()
    {
      Dispose(false);
    }


    /// <summary>
    /// Frees the native resources associated with this object
    /// </summary>
    public void Dispose()
    {
      Dispose(true);

      GC.SuppressFinalize(this);
    }


    /// <summary internal= "true">
    /// Internal dispose function
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
      if (!disposed)
      {
        disposed= true;

        if (disposing)
        {
          // Free managed resources;
        }

        // Free native resources
      }
    }


    #endregion Finalize

    #region Methods

    /// <summary>
    /// Provides a single output, 0-input MWArrayinterface to the combustible MATLAB
    /// function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// Definir distribucion inicial del combustible
    /// S_int = ones(size(U_int));
    /// </remarks>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray combustible()
    {
      return mcr.EvaluateFunction("combustible", new MWArray[]{});
    }


    /// <summary>
    /// Provides a single output, 1-input MWArrayinterface to the combustible MATLAB
    /// function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// Definir distribucion inicial del combustible
    /// S_int = ones(size(U_int));
    /// </remarks>
    /// <param name="A">Input argument #1</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray combustible(MWArray A)
    {
      return mcr.EvaluateFunction("combustible", A);
    }


    /// <summary>
    /// Provides the standard 0-input MWArray interface to the combustible MATLAB
    /// function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// Definir distribucion inicial del combustible
    /// S_int = ones(size(U_int));
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] combustible(int numArgsOut)
    {
      return mcr.EvaluateFunction(numArgsOut, "combustible", new MWArray[]{});
    }


    /// <summary>
    /// Provides the standard 1-input MWArray interface to the combustible MATLAB
    /// function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// Definir distribucion inicial del combustible
    /// S_int = ones(size(U_int));
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="A">Input argument #1</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] combustible(int numArgsOut, MWArray A)
    {
      return mcr.EvaluateFunction(numArgsOut, "combustible", A);
    }


    /// <summary>
    /// Provides an interface for the combustible function in which the input and output
    /// arguments are specified as an array of MWArrays.
    /// </summary>
    /// <remarks>
    /// This method will allocate and return by reference the output argument
    /// array.<newpara></newpara>
    /// M-Documentation:
    /// Definir distribucion inicial del combustible
    /// S_int = ones(size(U_int));
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return</param>
    /// <param name= "argsOut">Array of MWArray output arguments</param>
    /// <param name= "argsIn">Array of MWArray input arguments</param>
    ///
    public void combustible(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
    {
      mcr.EvaluateFunction("combustible", numArgsOut, ref argsOut, argsIn);
    }


    /// <summary>
    /// Provides a single output, 0-input MWArrayinterface to the fire MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// Parametros del modelo
    /// </remarks>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray fire()
    {
      return mcr.EvaluateFunction("fire", new MWArray[]{});
    }


    /// <summary>
    /// Provides a single output, 1-input MWArrayinterface to the fire MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// Parametros del modelo
    /// </remarks>
    /// <param name="U_in1">Input argument #1</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray fire(MWArray U_in1)
    {
      return mcr.EvaluateFunction("fire", U_in1);
    }


    /// <summary>
    /// Provides a single output, 2-input MWArrayinterface to the fire MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// Parametros del modelo
    /// </remarks>
    /// <param name="U_in1">Input argument #1</param>
    /// <param name="S_in1">Input argument #2</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray fire(MWArray U_in1, MWArray S_in1)
    {
      return mcr.EvaluateFunction("fire", U_in1, S_in1);
    }


    /// <summary>
    /// Provides a single output, 3-input MWArrayinterface to the fire MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// Parametros del modelo
    /// </remarks>
    /// <param name="U_in1">Input argument #1</param>
    /// <param name="S_in1">Input argument #2</param>
    /// <param name="W">Input argument #3</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray fire(MWArray U_in1, MWArray S_in1, MWArray W)
    {
      return mcr.EvaluateFunction("fire", U_in1, S_in1, W);
    }


    /// <summary>
    /// Provides a single output, 4-input MWArrayinterface to the fire MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// Parametros del modelo
    /// </remarks>
    /// <param name="U_in1">Input argument #1</param>
    /// <param name="S_in1">Input argument #2</param>
    /// <param name="W">Input argument #3</param>
    /// <param name="Tmax">Input argument #4</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray fire(MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax)
    {
      return mcr.EvaluateFunction("fire", U_in1, S_in1, W, Tmax);
    }


    /// <summary>
    /// Provides a single output, 5-input MWArrayinterface to the fire MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// Parametros del modelo
    /// </remarks>
    /// <param name="U_in1">Input argument #1</param>
    /// <param name="S_in1">Input argument #2</param>
    /// <param name="W">Input argument #3</param>
    /// <param name="Tmax">Input argument #4</param>
    /// <param name="Sfin">Input argument #5</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray fire(MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray 
                  Sfin)
    {
      return mcr.EvaluateFunction("fire", U_in1, S_in1, W, Tmax, Sfin);
    }


    /// <summary>
    /// Provides a single output, 6-input MWArrayinterface to the fire MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// Parametros del modelo
    /// </remarks>
    /// <param name="U_in1">Input argument #1</param>
    /// <param name="S_in1">Input argument #2</param>
    /// <param name="W">Input argument #3</param>
    /// <param name="Tmax">Input argument #4</param>
    /// <param name="Sfin">Input argument #5</param>
    /// <param name="alpha">Input argument #6</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray fire(MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray 
                  Sfin, MWArray alpha)
    {
      return mcr.EvaluateFunction("fire", U_in1, S_in1, W, Tmax, Sfin, alpha);
    }


    /// <summary>
    /// Provides a single output, 7-input MWArrayinterface to the fire MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// Parametros del modelo
    /// </remarks>
    /// <param name="U_in1">Input argument #1</param>
    /// <param name="S_in1">Input argument #2</param>
    /// <param name="W">Input argument #3</param>
    /// <param name="Tmax">Input argument #4</param>
    /// <param name="Sfin">Input argument #5</param>
    /// <param name="alpha">Input argument #6</param>
    /// <param name="k">Input argument #7</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray fire(MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray 
                  Sfin, MWArray alpha, MWArray k)
    {
      return mcr.EvaluateFunction("fire", U_in1, S_in1, W, Tmax, Sfin, alpha, k);
    }


    /// <summary>
    /// Provides a single output, 8-input MWArrayinterface to the fire MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// Parametros del modelo
    /// </remarks>
    /// <param name="U_in1">Input argument #1</param>
    /// <param name="S_in1">Input argument #2</param>
    /// <param name="W">Input argument #3</param>
    /// <param name="Tmax">Input argument #4</param>
    /// <param name="Sfin">Input argument #5</param>
    /// <param name="alpha">Input argument #6</param>
    /// <param name="k">Input argument #7</param>
    /// <param name="Tig">Input argument #8</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray fire(MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray 
                  Sfin, MWArray alpha, MWArray k, MWArray Tig)
    {
      return mcr.EvaluateFunction("fire", U_in1, S_in1, W, Tmax, Sfin, alpha, k, Tig);
    }


    /// <summary>
    /// Provides a single output, 9-input MWArrayinterface to the fire MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// Parametros del modelo
    /// </remarks>
    /// <param name="U_in1">Input argument #1</param>
    /// <param name="S_in1">Input argument #2</param>
    /// <param name="W">Input argument #3</param>
    /// <param name="Tmax">Input argument #4</param>
    /// <param name="Sfin">Input argument #5</param>
    /// <param name="alpha">Input argument #6</param>
    /// <param name="k">Input argument #7</param>
    /// <param name="Tig">Input argument #8</param>
    /// <param name="Cs">Input argument #9</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray fire(MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray 
                  Sfin, MWArray alpha, MWArray k, MWArray Tig, MWArray Cs)
    {
      return mcr.EvaluateFunction("fire", U_in1, S_in1, W, Tmax, Sfin, alpha, k, Tig, Cs);
    }


    /// <summary>
    /// Provides a single output, 10-input MWArrayinterface to the fire MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// Parametros del modelo
    /// </remarks>
    /// <param name="U_in1">Input argument #1</param>
    /// <param name="S_in1">Input argument #2</param>
    /// <param name="W">Input argument #3</param>
    /// <param name="Tmax">Input argument #4</param>
    /// <param name="Sfin">Input argument #5</param>
    /// <param name="alpha">Input argument #6</param>
    /// <param name="k">Input argument #7</param>
    /// <param name="Tig">Input argument #8</param>
    /// <param name="Cs">Input argument #9</param>
    /// <param name="ST">Input argument #10</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray fire(MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray 
                  Sfin, MWArray alpha, MWArray k, MWArray Tig, MWArray Cs, MWArray ST)
    {
      return mcr.EvaluateFunction("fire", U_in1, S_in1, W, Tmax, Sfin, alpha, k, Tig, Cs, ST);
    }


    /// <summary>
    /// Provides a single output, 11-input MWArrayinterface to the fire MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// Parametros del modelo
    /// </remarks>
    /// <param name="U_in1">Input argument #1</param>
    /// <param name="S_in1">Input argument #2</param>
    /// <param name="W">Input argument #3</param>
    /// <param name="Tmax">Input argument #4</param>
    /// <param name="Sfin">Input argument #5</param>
    /// <param name="alpha">Input argument #6</param>
    /// <param name="k">Input argument #7</param>
    /// <param name="Tig">Input argument #8</param>
    /// <param name="Cs">Input argument #9</param>
    /// <param name="ST">Input argument #10</param>
    /// <param name="d">Input argument #11</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray fire(MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray 
                  Sfin, MWArray alpha, MWArray k, MWArray Tig, MWArray Cs, MWArray ST, 
                  MWArray d)
    {
      return mcr.EvaluateFunction("fire", U_in1, S_in1, W, Tmax, Sfin, alpha, k, Tig, Cs, ST, d);
    }


    /// <summary>
    /// Provides the standard 0-input MWArray interface to the fire MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// Parametros del modelo
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] fire(int numArgsOut)
    {
      return mcr.EvaluateFunction(numArgsOut, "fire", new MWArray[]{});
    }


    /// <summary>
    /// Provides the standard 1-input MWArray interface to the fire MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// Parametros del modelo
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="U_in1">Input argument #1</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] fire(int numArgsOut, MWArray U_in1)
    {
      return mcr.EvaluateFunction(numArgsOut, "fire", U_in1);
    }


    /// <summary>
    /// Provides the standard 2-input MWArray interface to the fire MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// Parametros del modelo
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="U_in1">Input argument #1</param>
    /// <param name="S_in1">Input argument #2</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1)
    {
      return mcr.EvaluateFunction(numArgsOut, "fire", U_in1, S_in1);
    }


    /// <summary>
    /// Provides the standard 3-input MWArray interface to the fire MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// Parametros del modelo
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="U_in1">Input argument #1</param>
    /// <param name="S_in1">Input argument #2</param>
    /// <param name="W">Input argument #3</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1, MWArray W)
    {
      return mcr.EvaluateFunction(numArgsOut, "fire", U_in1, S_in1, W);
    }


    /// <summary>
    /// Provides the standard 4-input MWArray interface to the fire MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// Parametros del modelo
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="U_in1">Input argument #1</param>
    /// <param name="S_in1">Input argument #2</param>
    /// <param name="W">Input argument #3</param>
    /// <param name="Tmax">Input argument #4</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1, MWArray W, 
                    MWArray Tmax)
    {
      return mcr.EvaluateFunction(numArgsOut, "fire", U_in1, S_in1, W, Tmax);
    }


    /// <summary>
    /// Provides the standard 5-input MWArray interface to the fire MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// Parametros del modelo
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="U_in1">Input argument #1</param>
    /// <param name="S_in1">Input argument #2</param>
    /// <param name="W">Input argument #3</param>
    /// <param name="Tmax">Input argument #4</param>
    /// <param name="Sfin">Input argument #5</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1, MWArray W, 
                    MWArray Tmax, MWArray Sfin)
    {
      return mcr.EvaluateFunction(numArgsOut, "fire", U_in1, S_in1, W, Tmax, Sfin);
    }


    /// <summary>
    /// Provides the standard 6-input MWArray interface to the fire MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// Parametros del modelo
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="U_in1">Input argument #1</param>
    /// <param name="S_in1">Input argument #2</param>
    /// <param name="W">Input argument #3</param>
    /// <param name="Tmax">Input argument #4</param>
    /// <param name="Sfin">Input argument #5</param>
    /// <param name="alpha">Input argument #6</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1, MWArray W, 
                    MWArray Tmax, MWArray Sfin, MWArray alpha)
    {
      return mcr.EvaluateFunction(numArgsOut, "fire", U_in1, S_in1, W, Tmax, Sfin, alpha);
    }


    /// <summary>
    /// Provides the standard 7-input MWArray interface to the fire MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// Parametros del modelo
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="U_in1">Input argument #1</param>
    /// <param name="S_in1">Input argument #2</param>
    /// <param name="W">Input argument #3</param>
    /// <param name="Tmax">Input argument #4</param>
    /// <param name="Sfin">Input argument #5</param>
    /// <param name="alpha">Input argument #6</param>
    /// <param name="k">Input argument #7</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1, MWArray W, 
                    MWArray Tmax, MWArray Sfin, MWArray alpha, MWArray k)
    {
      return mcr.EvaluateFunction(numArgsOut, "fire", U_in1, S_in1, W, Tmax, Sfin, alpha, k);
    }


    /// <summary>
    /// Provides the standard 8-input MWArray interface to the fire MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// Parametros del modelo
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="U_in1">Input argument #1</param>
    /// <param name="S_in1">Input argument #2</param>
    /// <param name="W">Input argument #3</param>
    /// <param name="Tmax">Input argument #4</param>
    /// <param name="Sfin">Input argument #5</param>
    /// <param name="alpha">Input argument #6</param>
    /// <param name="k">Input argument #7</param>
    /// <param name="Tig">Input argument #8</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1, MWArray W, 
                    MWArray Tmax, MWArray Sfin, MWArray alpha, MWArray k, MWArray Tig)
    {
      return mcr.EvaluateFunction(numArgsOut, "fire", U_in1, S_in1, W, Tmax, Sfin, alpha, k, Tig);
    }


    /// <summary>
    /// Provides the standard 9-input MWArray interface to the fire MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// Parametros del modelo
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="U_in1">Input argument #1</param>
    /// <param name="S_in1">Input argument #2</param>
    /// <param name="W">Input argument #3</param>
    /// <param name="Tmax">Input argument #4</param>
    /// <param name="Sfin">Input argument #5</param>
    /// <param name="alpha">Input argument #6</param>
    /// <param name="k">Input argument #7</param>
    /// <param name="Tig">Input argument #8</param>
    /// <param name="Cs">Input argument #9</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1, MWArray W, 
                    MWArray Tmax, MWArray Sfin, MWArray alpha, MWArray k, MWArray Tig, 
                    MWArray Cs)
    {
      return mcr.EvaluateFunction(numArgsOut, "fire", U_in1, S_in1, W, Tmax, Sfin, alpha, k, Tig, Cs);
    }


    /// <summary>
    /// Provides the standard 10-input MWArray interface to the fire MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// Parametros del modelo
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="U_in1">Input argument #1</param>
    /// <param name="S_in1">Input argument #2</param>
    /// <param name="W">Input argument #3</param>
    /// <param name="Tmax">Input argument #4</param>
    /// <param name="Sfin">Input argument #5</param>
    /// <param name="alpha">Input argument #6</param>
    /// <param name="k">Input argument #7</param>
    /// <param name="Tig">Input argument #8</param>
    /// <param name="Cs">Input argument #9</param>
    /// <param name="ST">Input argument #10</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1, MWArray W, 
                    MWArray Tmax, MWArray Sfin, MWArray alpha, MWArray k, MWArray Tig, 
                    MWArray Cs, MWArray ST)
    {
      return mcr.EvaluateFunction(numArgsOut, "fire", U_in1, S_in1, W, Tmax, Sfin, alpha, k, Tig, Cs, ST);
    }


    /// <summary>
    /// Provides the standard 11-input MWArray interface to the fire MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// Parametros del modelo
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="U_in1">Input argument #1</param>
    /// <param name="S_in1">Input argument #2</param>
    /// <param name="W">Input argument #3</param>
    /// <param name="Tmax">Input argument #4</param>
    /// <param name="Sfin">Input argument #5</param>
    /// <param name="alpha">Input argument #6</param>
    /// <param name="k">Input argument #7</param>
    /// <param name="Tig">Input argument #8</param>
    /// <param name="Cs">Input argument #9</param>
    /// <param name="ST">Input argument #10</param>
    /// <param name="d">Input argument #11</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1, MWArray W, 
                    MWArray Tmax, MWArray Sfin, MWArray alpha, MWArray k, MWArray Tig, 
                    MWArray Cs, MWArray ST, MWArray d)
    {
      return mcr.EvaluateFunction(numArgsOut, "fire", U_in1, S_in1, W, Tmax, Sfin, alpha, k, Tig, Cs, ST, d);
    }


    /// <summary>
    /// Provides an interface for the fire function in which the input and output
    /// arguments are specified as an array of MWArrays.
    /// </summary>
    /// <remarks>
    /// This method will allocate and return by reference the output argument
    /// array.<newpara></newpara>
    /// M-Documentation:
    /// Parametros del modelo
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return</param>
    /// <param name= "argsOut">Array of MWArray output arguments</param>
    /// <param name= "argsIn">Array of MWArray input arguments</param>
    ///
    public void fire(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
    {
      mcr.EvaluateFunction("fire", numArgsOut, ref argsOut, argsIn);
    }


    /// <summary>
    /// Provides a single output, 0-input MWArrayinterface to the focos MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// generacion aleatoria de fuegos
    /// -----------------------------------
    /// Constantes termodinamicas
    /// -----------------------------------
    /// </remarks>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray focos()
    {
      return mcr.EvaluateFunction("focos", new MWArray[]{});
    }


    /// <summary>
    /// Provides a single output, 1-input MWArrayinterface to the focos MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// generacion aleatoria de fuegos
    /// -----------------------------------
    /// Constantes termodinamicas
    /// -----------------------------------
    /// </remarks>
    /// <param name="A">Input argument #1</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray focos(MWArray A)
    {
      return mcr.EvaluateFunction("focos", A);
    }


    /// <summary>
    /// Provides a single output, 2-input MWArrayinterface to the focos MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// generacion aleatoria de fuegos
    /// -----------------------------------
    /// Constantes termodinamicas
    /// -----------------------------------
    /// </remarks>
    /// <param name="A">Input argument #1</param>
    /// <param name="focos">Input argument #2</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray focos(MWArray A, MWArray focos)
    {
      return mcr.EvaluateFunction("focos", A, focos);
    }


    /// <summary>
    /// Provides the standard 0-input MWArray interface to the focos MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// generacion aleatoria de fuegos
    /// -----------------------------------
    /// Constantes termodinamicas
    /// -----------------------------------
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] focos(int numArgsOut)
    {
      return mcr.EvaluateFunction(numArgsOut, "focos", new MWArray[]{});
    }


    /// <summary>
    /// Provides the standard 1-input MWArray interface to the focos MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// generacion aleatoria de fuegos
    /// -----------------------------------
    /// Constantes termodinamicas
    /// -----------------------------------
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="A">Input argument #1</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] focos(int numArgsOut, MWArray A)
    {
      return mcr.EvaluateFunction(numArgsOut, "focos", A);
    }


    /// <summary>
    /// Provides the standard 2-input MWArray interface to the focos MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// generacion aleatoria de fuegos
    /// -----------------------------------
    /// Constantes termodinamicas
    /// -----------------------------------
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="A">Input argument #1</param>
    /// <param name="focos">Input argument #2</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] focos(int numArgsOut, MWArray A, MWArray focos)
    {
      return mcr.EvaluateFunction(numArgsOut, "focos", A, focos);
    }


    /// <summary>
    /// Provides an interface for the focos function in which the input and output
    /// arguments are specified as an array of MWArrays.
    /// </summary>
    /// <remarks>
    /// This method will allocate and return by reference the output argument
    /// array.<newpara></newpara>
    /// M-Documentation:
    /// generacion aleatoria de fuegos
    /// -----------------------------------
    /// Constantes termodinamicas
    /// -----------------------------------
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return</param>
    /// <param name= "argsOut">Array of MWArray output arguments</param>
    /// <param name= "argsIn">Array of MWArray input arguments</param>
    ///
    public void focos(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
    {
      mcr.EvaluateFunction("focos", numArgsOut, ref argsOut, argsIn);
    }


    /// <summary>
    /// Provides a single output, 0-input MWArrayinterface to the matriz MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// A = ones(n,n);
    /// </remarks>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray matriz()
    {
      return mcr.EvaluateFunction("matriz", new MWArray[]{});
    }


    /// <summary>
    /// Provides a single output, 1-input MWArrayinterface to the matriz MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// A = ones(n,n);
    /// </remarks>
    /// <param name="Lx">Input argument #1</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray matriz(MWArray Lx)
    {
      return mcr.EvaluateFunction("matriz", Lx);
    }


    /// <summary>
    /// Provides a single output, 2-input MWArrayinterface to the matriz MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// A = ones(n,n);
    /// </remarks>
    /// <param name="Lx">Input argument #1</param>
    /// <param name="Ly">Input argument #2</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray matriz(MWArray Lx, MWArray Ly)
    {
      return mcr.EvaluateFunction("matriz", Lx, Ly);
    }


    /// <summary>
    /// Provides a single output, 3-input MWArrayinterface to the matriz MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// A = ones(n,n);
    /// </remarks>
    /// <param name="Lx">Input argument #1</param>
    /// <param name="Ly">Input argument #2</param>
    /// <param name="d">Input argument #3</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray matriz(MWArray Lx, MWArray Ly, MWArray d)
    {
      return mcr.EvaluateFunction("matriz", Lx, Ly, d);
    }


    /// <summary>
    /// Provides the standard 0-input MWArray interface to the matriz MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// A = ones(n,n);
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] matriz(int numArgsOut)
    {
      return mcr.EvaluateFunction(numArgsOut, "matriz", new MWArray[]{});
    }


    /// <summary>
    /// Provides the standard 1-input MWArray interface to the matriz MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// A = ones(n,n);
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="Lx">Input argument #1</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] matriz(int numArgsOut, MWArray Lx)
    {
      return mcr.EvaluateFunction(numArgsOut, "matriz", Lx);
    }


    /// <summary>
    /// Provides the standard 2-input MWArray interface to the matriz MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// A = ones(n,n);
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="Lx">Input argument #1</param>
    /// <param name="Ly">Input argument #2</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] matriz(int numArgsOut, MWArray Lx, MWArray Ly)
    {
      return mcr.EvaluateFunction(numArgsOut, "matriz", Lx, Ly);
    }


    /// <summary>
    /// Provides the standard 3-input MWArray interface to the matriz MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// A = ones(n,n);
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="Lx">Input argument #1</param>
    /// <param name="Ly">Input argument #2</param>
    /// <param name="d">Input argument #3</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] matriz(int numArgsOut, MWArray Lx, MWArray Ly, MWArray d)
    {
      return mcr.EvaluateFunction(numArgsOut, "matriz", Lx, Ly, d);
    }


    /// <summary>
    /// Provides an interface for the matriz function in which the input and output
    /// arguments are specified as an array of MWArrays.
    /// </summary>
    /// <remarks>
    /// This method will allocate and return by reference the output argument
    /// array.<newpara></newpara>
    /// M-Documentation:
    /// A = ones(n,n);
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return</param>
    /// <param name= "argsOut">Array of MWArray output arguments</param>
    /// <param name= "argsIn">Array of MWArray input arguments</param>
    ///
    public void matriz(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
    {
      mcr.EvaluateFunction("matriz", numArgsOut, ref argsOut, argsIn);
    }



    /// <summary>
    /// This method will cause a MATLAB figure window to behave as a modal dialog box.
    /// The method will not return until all the figure windows associated with this
    /// component have been closed.
    /// </summary>
    /// <remarks>
    /// An application should only call this method when required to keep the
    /// MATLAB figure window from disappearing.  Other techniques, such as calling
    /// Console.ReadLine() from the application should be considered where
    /// possible.</remarks>
    ///
    public void WaitForFiguresToDie()
    {
      mcr.WaitForFiguresToDie();
    }



    #endregion Methods

    #region Class Members

    private static MWMCR mcr= null;

    private static Exception ex_= null;

    private bool disposed= false;

    #endregion Class Members
  }
}
