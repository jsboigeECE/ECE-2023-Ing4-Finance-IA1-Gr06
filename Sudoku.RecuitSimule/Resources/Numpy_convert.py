import ctypes

import numpy as np

import clr 
import System
from System import Array, Int32
from System.Runtime.InteropServices import GCHandle, GCHandleType

_MAP_NP_NET = {
    np.dtype(np.float32): System.Single,
    np.dtype(np.float64): System.Double,
    np.dtype(np.int8)   : System.SByte,
    np.dtype(np.int16)  : System.Int16,
    np.dtype(np.int32)  : System.Int32,
    np.dtype(np.int64)  : System.Int64,
    np.dtype(np.uint8)  : System.Byte,
    np.dtype(np.uint16) : System.UInt16,
    np.dtype(np.uint32) : System.UInt32,
    np.dtype(np.uint64) : System.UInt64,
    np.dtype(np.bool)   : System.Boolean,
}
_MAP_NET_NP = {
    'Single' : np.dtype(np.float32),
    'Double' : np.dtype(np.float64),
    'SByte'  : np.dtype(np.int8),
    'Int16'  : np.dtype(np.int16), 
    'Int32'  : np.dtype(np.int32),
    'Int64'  : np.dtype(np.int64),
    'Byte'   : np.dtype(np.uint8),
    'UInt16' : np.dtype(np.uint16),
    'UInt32' : np.dtype(np.uint32),
    'UInt64' : np.dtype(np.uint64),
    'Boolean': np.dtype(np.bool),
}


def asNetArray(npArray):
    """
    Converts a NumPy array to a .NET array. See `_MAP_NP_NET` for 
    the mapping of CLR types to Numpy ``dtype``.

    Parameters
    ----------
    npArray: numpy.ndarray
        The array to be converted

    Returns
    -------
    System.Array

    Warning
    -------
    ``complex64`` and ``complex128`` arrays are converted to ``float32``
    and ``float64`` arrays respectively with shape ``[m,n,...] -> [m,n,...,2]``

    """
    dims = npArray.shape
    dtype = npArray.dtype

    # For complex arrays, we must make a view of the array as its corresponding 
    # float type as if it's (real, imag)
    if dtype == numpy.complex64:
        dtype = numpy.dtype(numpy.float32)
        dims += (2,)
        npArray = npArray.view(dtype).reshape(dims)
    elif dtype == numpy.complex128:
        dtype = numpy.dtype(numpy.float64)
        dims += (2,)
        npArray = npArray.view(dtype).reshape(dims)

    if not npArray.flags.c_contiguous or not npArray.flags.aligned:
        npArray = numpy.ascontiguousarray(npArray)
    assert npArray.flags.c_contiguous

    try:
        netArray = Array.CreateInstance(_MAP_NP_NET[dtype], *dims)
    except KeyError:
        raise NotImplementedError(f'asNetArray does not yet support dtype {dtype}')

    try: # Memmove 
        destHandle = GCHandle.Alloc(netArray, GCHandleType.Pinned)
        sourcePtr = npArray.__array_interface__['data'][0]
        destPtr = destHandle.AddrOfPinnedObject().ToInt64()
        ctypes.memmove(destPtr, sourcePtr, npArray.nbytes)
    finally:
        if destHandle.IsAllocated: 
            destHandle.Free()
    return netArray