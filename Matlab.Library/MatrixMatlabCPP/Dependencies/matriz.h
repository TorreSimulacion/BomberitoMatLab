//
// MATLAB Compiler: 7.0.1 (R2019a)
// Date: Wed Aug 21 10:21:48 2019
// Arguments:
// "-B""macro_default""-W""cpplib:matriz,all""-T""link:lib""-d""C:\Users\yapmrq\
// Documents\MATLAB\Prevencion de
// Incendio\fire12\bin\c++\for_testing""-v""C:\Users\yapmrq\Documents\MATLAB\Pre
// vencion de Incendio\fire12\matriz.m"
//

#ifndef __matriz_h
#define __matriz_h 1

#if defined(__cplusplus) && !defined(mclmcrrt_h) && defined(__linux__)
#  pragma implementation "mclmcrrt.h"
#endif
#include "mclmcrrt.h"
#include "mclcppclass.h"
#ifdef __cplusplus
extern "C" {
#endif

/* This symbol is defined in shared libraries. Define it here
 * (to nothing) in case this isn't a shared library. 
 */
#ifndef LIB_matriz_C_API 
#define LIB_matriz_C_API /* No special import/export declaration */
#endif

/* GENERAL LIBRARY FUNCTIONS -- START */

extern LIB_matriz_C_API 
bool MW_CALL_CONV matrizInitializeWithHandlers(
       mclOutputHandlerFcn error_handler, 
       mclOutputHandlerFcn print_handler);

extern LIB_matriz_C_API 
bool MW_CALL_CONV matrizInitialize(void);

extern LIB_matriz_C_API 
void MW_CALL_CONV matrizTerminate(void);

extern LIB_matriz_C_API 
void MW_CALL_CONV matrizPrintStackTrace(void);

/* GENERAL LIBRARY FUNCTIONS -- END */

/* C INTERFACE -- MLX WRAPPERS FOR USER-DEFINED MATLAB FUNCTIONS -- START */

extern LIB_matriz_C_API 
bool MW_CALL_CONV mlxMatriz(int nlhs, mxArray *plhs[], int nrhs, mxArray *prhs[]);

/* C INTERFACE -- MLX WRAPPERS FOR USER-DEFINED MATLAB FUNCTIONS -- END */

#ifdef __cplusplus
}
#endif


/* C++ INTERFACE -- WRAPPERS FOR USER-DEFINED MATLAB FUNCTIONS -- START */

#ifdef __cplusplus

/* On Windows, use __declspec to control the exported API */
#if defined(_MSC_VER) || defined(__MINGW64__)

#ifdef EXPORTING_matriz
#define PUBLIC_matriz_CPP_API __declspec(dllexport)
#else
#define PUBLIC_matriz_CPP_API __declspec(dllimport)
#endif

#define LIB_matriz_CPP_API PUBLIC_matriz_CPP_API

#else

#if !defined(LIB_matriz_CPP_API)
#if defined(LIB_matriz_C_API)
#define LIB_matriz_CPP_API LIB_matriz_C_API
#else
#define LIB_matriz_CPP_API /* empty! */ 
#endif
#endif

#endif

extern LIB_matriz_CPP_API void MW_CALL_CONV matriz(int nargout, mwArray& A, const mwArray& n);

/* C++ INTERFACE -- WRAPPERS FOR USER-DEFINED MATLAB FUNCTIONS -- END */
#endif

#endif
