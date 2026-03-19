using System;
using System.Runtime.InteropServices;

namespace CommonUtility
{
    public enum CheetahStatus
    {
        /* General codes (0 to -99) */
        ChOk = 0,
        ChUnableToLoadLibrary = -1,
        ChUnableToLoadDriver = -2,
        ChUnableToLoadFunction = -3,
        ChIncompatibleLibrary = -4,
        ChIncompatibleDevice = -5,
        ChIncompatibleDriver = -6,
        ChCommunicationError = -7,
        ChUnableToOpen = -8,
        ChUnableToClose = -9,
        ChInvalidHandle = -10,
        ChConfigError = -11,
        ChUnknownProtocol = -12,
        ChStillActive = -13,
        ChFunctionNotAvailable = -14,
        ChOsError = -15,

        /* SPI codes (-100 to -199) */
        ChSpiWriteError = -100,
        ChSpiBatchEmptyQueue = -101,
        ChSpiBatchShortBuffer = -102,
        ChSpiAsyncEmpty = -103,
        ChSpiAsyncPending = -104,
        ChSpiAsyncMaxReached = -105,
        ChSpiAsyncExcessDelay = -106
    }

    public enum CheetahSpiPolarity
    {
        ChSpiPolRisingFalling = 0,
        ChSpiPolFallingRising = 1
    }

    public enum CheetahSpiPhase
    {
        ChSpiPhaseSampleSetup = 0,
        ChSpiPhaseSetupSample = 1
    }

    public enum CheetahSpiBitorder
    {
        ChSpiBitorderMsb = 0,
        ChSpiBitorderLsb = 1
    }

    public class CheetahApi
    {
        public const string DllFilename = @"\DllImport\cheetah.dll";

        /*=========================================================================
        | HELPER FUNCTIONS / CLASSES
         ========================================================================*/
        private static long tp_min(long x, long y) { return x < y ? x : y; }

        private class GcContext
        {
            private readonly GCHandle[] _handles;
            private int _index;

            public GcContext()
            {
                _handles = new GCHandle[16];
                _index = 0;
            }

            public void Add(GCHandle gch)
            {
                _handles[_index] = gch;
                _index++;
            }

            public void Free()
            {
                while (_index != 0)
                {
                    _index--;
                    _handles[_index].Free();
                }
            }
        }

        /*=========================================================================
        | VERSION
         ========================================================================*/
        [DllImport(DllFilename)]
        private static extern int ch_c_version();

        public const int ChApiVersion = 0x0332;   // v3.50
        public const int ChReqSwVersion = 0x0306;   // v3.06

        public static readonly short ChSwVersion;
        public static readonly short ChReqApiVersion;
        public static readonly bool ChLibraryLoaded;

        static CheetahApi()
        {
            ChSwVersion = (short)(ch_c_version() & 0xffff);
            ChReqApiVersion = (short)((ch_c_version() >> 16) & 0xffff);
            ChLibraryLoaded =
                ((ChSwVersion >= ChReqSwVersion) &&
                 (ChApiVersion >= ChReqApiVersion));
        }

        /*=========================================================================
        | STATUS CODES
         ========================================================================*/
        /*
         * All API functions return an integer which is the result of the
         * transaction, or a status code if negative.  The status codes are
         * defined as follows:
         */
        // enum CheetahStatus  (from declaration above)
        //     CH_OK                      =    0
        //     CH_UNABLE_TO_LOAD_LIBRARY  =   -1
        //     CH_UNABLE_TO_LOAD_DRIVER   =   -2
        //     CH_UNABLE_TO_LOAD_FUNCTION =   -3
        //     CH_INCOMPATIBLE_LIBRARY    =   -4
        //     CH_INCOMPATIBLE_DEVICE     =   -5
        //     CH_INCOMPATIBLE_DRIVER     =   -6
        //     CH_COMMUNICATION_ERROR     =   -7
        //     CH_UNABLE_TO_OPEN          =   -8
        //     CH_UNABLE_TO_CLOSE         =   -9
        //     CH_INVALID_HANDLE          =  -10
        //     CH_CONFIG_ERROR            =  -11
        //     CH_UNKNOWN_PROTOCOL        =  -12
        //     CH_STILL_ACTIVE            =  -13
        //     CH_FUNCTION_NOT_AVAILABLE  =  -14
        //     CH_OS_ERROR                =  -15
        //     CH_SPI_WRITE_ERROR         = -100
        //     CH_SPI_BATCH_EMPTY_QUEUE   = -101
        //     CH_SPI_BATCH_SHORT_BUFFER  = -102
        //     CH_SPI_ASYNC_EMPTY         = -103
        //     CH_SPI_ASYNC_PENDING       = -104
        //     CH_SPI_ASYNC_MAX_REACHED   = -105
        //     CH_SPI_ASYNC_EXCESS_DELAY  = -106


        /*=========================================================================
        | GENERAL TYPE DEFINITIONS
         ========================================================================*/
        /* Cheetah handle type definition */
        /* typedef Cheetah => int */

        /*
         * Cheetah version matrix.
         *
         * This matrix describes the various version dependencies
         * of Cheetah components.  It can be used to determine
         * which component caused an incompatibility error.
         *
         * All version numbers are of the format:
         *   (major << 8) | minor
         *
         * ex. v1.20 would be encoded as:  0x0114
         */
        [StructLayout(LayoutKind.Sequential)]
        public struct CheetahVersion
        {
            /* Software, firmware, and hardware versions. */
            public ushort software;
            public ushort firmware;
            public ushort hardware;

            /*
             * Hardware revisions that are compatible with this software version.
             * The top 16 bits gives the maximum accepted hardware revision.
             * The lower 16 bits gives the minimum accepted hardware revision.
             */
            public uint hw_revs_for_sw;

            /*
             * Firmware revisions that are compatible with this software version.
             * The top 16 bits gives the maximum accepted fw revision.
             * The lower 16 bits gives the minimum accepted fw revision.
             */
            public uint fw_revs_for_sw;

            /*
             * Driver revisions that are compatible with this software version.
             * The top 16 bits gives the maximum accepted driver revision.
             * The lower 16 bits gives the minimum accepted driver revision.
             * This version checking is currently only pertinent for WIN32
             * platforms.
             */
            public uint drv_revs_for_sw;

            /* Software requires that the API interface must be >= this version. */
            public ushort api_req_by_sw;
        }


        /*=========================================================================
        | GENERAL API
         ========================================================================*/
        /*
         * Get a list of ports to which Cheetah devices are attached.
         *
         * num_devices = maximum number of elements to return
         * devices     = array into which the port numbers are returned
         *
         * Each element of the array is written with the port number.
         * Devices that are in-use are ORed with CH_PORT_NOT_FREE
         * (0x8000).
         *
         * ex.  devices are attached to ports 0, 1, 2
         *      ports 0 and 2 are available, and port 1 is in-use.
         *      array => 0x0000, 0x8001, 0x0002
         *
         * If the array is NULL, it is not filled with any values.
         * If there are more devices than the array size, only the
         * first nmemb port numbers will be written into the array.
         *
         * Returns the number of devices found, regardless of the
         * array size.
         */
        public static ushort ChPortNotFree = 0x8000;
        public static int ch_find_devices(
            int numDevices, ushort[] devices)
        {
            if (!ChLibraryLoaded) return (int)CheetahStatus.ChIncompatibleLibrary;
            var devicesNumDevices = (int)tp_min(numDevices, devices.Length);
            return net_ch_find_devices(devicesNumDevices, devices);
        }

        /*
         * Get a list of ports to which Cheetah devices are attached
         *
         * This function is the same as ch_find_devices() except that
         * it returns the unique IDs of each Cheetah device.  The IDs
         * are guaranteed to be non-zero if valid.
         *
         * The IDs are the unsigned integer representation of the 10-digit
         * serial numbers.
         */
        public static int ch_find_devices_ext(
            int numDevices, ushort[] devices, int numIds, uint[] uniqueIds)
        {
            if (!ChLibraryLoaded) return (int)CheetahStatus.ChIncompatibleLibrary;
            var devicesNumDevices = (int)tp_min(numDevices, devices.Length);
            var uniqueIdsNumIds = (int)tp_min(numIds, uniqueIds.Length);
            return net_ch_find_devices_ext(devicesNumDevices, devices, uniqueIdsNumIds, uniqueIds);
        }

        /*
         * Open the Cheetah port.
         *
         * The port number is a zero-indexed integer.
         *
         * The port number is the same as that obtained from the
         * ch_find_devices() function above.
         *
         * Returns an Cheetah handle, which is guaranteed to be
         * greater than zero if it is valid.
         *
         * This function is recommended for use in simple applications
         * where extended information is not required.  For more complex
         * applications, the use of ch_open_ext() is recommended.
         */
        public static int ch_open(
            int portNumber)
        {
            if (!ChLibraryLoaded) return (int)CheetahStatus.ChIncompatibleLibrary;
            return net_ch_open(portNumber);
        }

        /*
         * Open the Cheetah port, returning extended information
         * in the supplied structure.  Behavior is otherwise identical
         * to ch_open() above.  If 0 is passed as the pointer to the
         * structure, this function is exactly equivalent to ch_open().
         *
         * The structure is zeroed before the open is attempted.
         * It is filled with whatever information is available.
         *
         * For example, if the hardware version is not filled, then
         * the device could not be queried for its version number.
         *
         * This function is recommended for use in complex applications
         * where extended information is required.  For more simple
         * applications, the use of ch_open() is recommended.
         */
        [StructLayout(LayoutKind.Sequential)]
        public struct CheetahExt
        {
            /* Version matrix */
            public CheetahVersion version;

            /* Features of this device. */
            public int features;
        }

        public static int ch_open_ext(
            int portNumber, ref CheetahExt chExt)
        {
            if (!ChLibraryLoaded) return (int)CheetahStatus.ChIncompatibleLibrary;
            return net_ch_open_ext(portNumber, ref chExt);
        }

        /* Close the Cheetah port. */
        public static int ch_close(
            int cheetah
        )
        {
            if (!ChLibraryLoaded) return (int)CheetahStatus.ChIncompatibleLibrary;
            return net_ch_close(cheetah);
        }

        /*
         * Return the port for this Cheetah handle.
         *
         * The port number is a zero-indexed integer.
         */
        public static int ch_port(
            int cheetah
        )
        {
            if (!ChLibraryLoaded) return (int)CheetahStatus.ChIncompatibleLibrary;
            return net_ch_port(cheetah);
        }

        /*
         * Return the unique ID for this Cheetah adapter.
         * IDs are guaranteed to be non-zero if valid.
         * The ID is the unsigned integer representation of the
         * 10-digit serial number.
         */
        public static uint ch_unique_id(
            int cheetah
        )
        {
            if (!ChLibraryLoaded) return 0;
            return net_ch_unique_id(cheetah);
        }

        /*
         * Return the status string for the given status code.
         * If the code is not valid or the library function cannot
         * be loaded, return a NULL string.
         */
        public static string ch_status_string(
            int status
        )
        {
            return !ChLibraryLoaded ? null : Marshal.PtrToStringAnsi(net_ch_status_string(status));
        }

        /*
         * Return the version matrix for the device attached to the
         * given handle.  If the handle is 0 or invalid, only the
         * software and required api versions are set.
         */
        public static int ch_version(
            int cheetah,
            ref CheetahVersion version
        )
        {
            if (!ChLibraryLoaded) return (int)CheetahStatus.ChIncompatibleLibrary;
            return net_ch_version(cheetah, ref version);
        }

        /*
         * Sleep for the specified number of milliseconds
         * Accuracy depends on the operating system scheduler
         * Returns the number of milliseconds slept
         */
        public static uint ch_sleep_ms(
            uint milliseconds
        )
        {
            return !ChLibraryLoaded ? 0 : net_ch_sleep_ms(milliseconds);
        }

        /* Configure the target power pin. */
        public const byte ChTargetPowerOff = 0x00;
        public const byte ChTargetPowerOn = 0x01;
        public const byte ChTargetPowerQuery = 0x80;

        public static int ch_target_power(
            int cheetah, byte powerFlag)
        {
            if (!ChLibraryLoaded) return (int)CheetahStatus.ChIncompatibleLibrary;
            return net_ch_target_power(cheetah, powerFlag);
        }

        public const byte ChHostIfceFullSpeed = 0x00;
        public const byte ChHostIfceHighSpeed = 0x01;
        public static int ch_host_ifce_speed(
            int cheetah
        )
        {
            if (!ChLibraryLoaded) return (int)CheetahStatus.ChIncompatibleLibrary;
            return net_ch_host_ifce_speed(cheetah);
        }

        /* Returns the device address that the beagle is attached to. */
        public static int ch_dev_addr(
            int cheetah
        )
        {
            if (!ChLibraryLoaded) return (int)CheetahStatus.ChIncompatibleLibrary;
            return net_ch_dev_addr(cheetah);
        }


        /*=========================================================================
        | SPI API
         ========================================================================*/
        /* Set the SPI bit rate in kilohertz. */
        public static int ch_spi_bitrate(
            int cheetah, int bitrateKhz)
        {
            if (!ChLibraryLoaded) return (int)CheetahStatus.ChIncompatibleLibrary;
            return net_ch_spi_bitrate(cheetah, bitrateKhz);
        }

        /*
         * These configuration parameters specify how to clock the
         * bits that are sent and received on the Cheetah SPI
         * interface.
         *
         * The polarity option specifies which transition
         * constitutes the leading edge and which transition is the
         * falling edge.  For example, CH_SPI_POL_RISING_FALLING
         * would configure the SPI to idle the SCK clock line low.
         * The clock would then transition low-to-high on the
         * leading edge and high-to-low on the trailing edge.
         *
         * The phase option determines whether to sample or setup on
         * the leading edge.  For example, CH_SPI_PHASE_SAMPLE_SETUP
         * would configure the SPI to sample on the leading edge and
         * setup on the trailing edge.
         *
         * The bitorder option is used to indicate whether LSB or
         * MSB is shifted first.
         *
         * The SS polarity option is to indicate the polarity of the
         * slave select pin (active high or active low).  Each of
         * the lower three bits of ss_polarity corresponds to each
         * of the SS lines.  Set the bit value for a given SS line
         * to 0 for active low or 1 for active high.
         */
        // enum CheetahSpiPolarity  (from declaration above)
        //     CH_SPI_POL_RISING_FALLING = 0
        //     CH_SPI_POL_FALLING_RISING = 1

        // enum CheetahSpiPhase  (from declaration above)
        //     CH_SPI_PHASE_SAMPLE_SETUP = 0
        //     CH_SPI_PHASE_SETUP_SAMPLE = 1

        // enum CheetahSpiBitorder  (from declaration above)
        //     CH_SPI_BITORDER_MSB = 0
        //     CH_SPI_BITORDER_LSB = 1

        /* Configure the SPI master interface */
        public static int ch_spi_configure(
            int cheetah,
            CheetahSpiPolarity polarity,
            CheetahSpiPhase phase,
            CheetahSpiBitorder bitorder,
            byte ssPolarity)
        {
            if (!ChLibraryLoaded) return (int)CheetahStatus.ChIncompatibleLibrary;
            return net_ch_spi_configure(cheetah, polarity, phase, bitorder, ssPolarity);
        }

        /* Clear the batch queue */
        public static int ch_spi_queue_clear(
            int cheetah
        )
        {
            if (!ChLibraryLoaded) return (int)CheetahStatus.ChIncompatibleLibrary;
            return net_ch_spi_queue_clear(cheetah);
        }

        /* Enable / disable the master outputs */
        public static int ch_spi_queue_oe(
            int cheetah,
            byte oe
        )
        {
            if (!ChLibraryLoaded) return (int)CheetahStatus.ChIncompatibleLibrary;
            return net_ch_spi_queue_oe(cheetah, oe);
        }

        /*
         * Queue a delay in clock cycles
         * The return value is the actual number of cycles queued.
         */
        public static int ch_spi_queue_delay_cycles(
            int cheetah,
            int cycles
        )
        {
            if (!ChLibraryLoaded) return (int)CheetahStatus.ChIncompatibleLibrary;
            return net_ch_spi_queue_delay_cycles(cheetah, cycles);
        }

        /*
         * Queue a delay in nanoseconds
         * The return value is the approximate number of nanoseconds queued.
         */
        public static int ch_spi_queue_delay_ns(
            int cheetah,
            int nanoseconds
        )
        {
            if (!ChLibraryLoaded) return (int)CheetahStatus.ChIncompatibleLibrary;
            return net_ch_spi_queue_delay_ns(cheetah, nanoseconds);
        }

        /*
         * Assert the slave select lines.  Each of the lower three
         * bits of ss_polarity corresponds to each of the SS lines.
         * Set the bit value for a given SS line to 1 to assert the
         * line or 0 to deassert the line.  The polarity is determined
         * by ch_spi_configure() above.
         */
        public static int ch_spi_queue_ss(
            int cheetah,
            byte active
        )
        {
            if (!ChLibraryLoaded) return (int)CheetahStatus.ChIncompatibleLibrary;
            return net_ch_spi_queue_ss(cheetah, active);
        }

        /* Repeatedly send a single byte */
        public static int ch_spi_queue_byte(
            int cheetah,
            int count,
            byte data
        )
        {
            if (!ChLibraryLoaded) return (int)CheetahStatus.ChIncompatibleLibrary;
            return net_ch_spi_queue_byte(cheetah, count, data);
        }

        /*
         * Send a byte array.  Repeated bytes are automatically
         * optimized into a repeated byte sequence.
         */
        public static int ch_spi_queue_array(
            int cheetah,int numBytes,byte[] dataOut)
        {
            if (!ChLibraryLoaded) return (int)CheetahStatus.ChIncompatibleLibrary;
            var dataOutNumBytes = (int)tp_min(numBytes, dataOut.Length);
            return net_ch_spi_queue_array(cheetah, dataOutNumBytes, dataOut);
        }

        /* Calculate the expected length of the returned data */
        public static int ch_spi_batch_length(int cheetah)
        {
            if (!ChLibraryLoaded) return (int)CheetahStatus.ChIncompatibleLibrary;
            return net_ch_spi_batch_length(cheetah);
        }

        /*
         * Perform the SPI shift operation.
         *
         * After the operation completes, the batch queue is untouched.
         * Therefore, this function may be called repeatedly in rapid
         * succession.
         */
        public static int ch_spi_batch_shift(
            int cheetah,int numBytes,byte[] dataIn
        )
        {
            if (!ChLibraryLoaded) return (int)CheetahStatus.ChIncompatibleLibrary;
            var dataInNumBytes = (int)tp_min(numBytes, dataIn.Length);
            return net_ch_spi_batch_shift(cheetah, dataInNumBytes, dataIn);
        }

        /*
         * Queue the current batch queue for asynchronous shifting.
         *
         * After the operation completes, the batch queue is untouched.
         * Therefore, this function may be called repeatedly in rapid
         * succession.
         */
        public static int ch_spi_async_submit(int cheetah)
        {
            if (!ChLibraryLoaded) return (int)CheetahStatus.ChIncompatibleLibrary;
            return net_ch_spi_async_submit(cheetah);
        }

        /* Collect an asynchronous batch that was previously submitted. */
        public static int ch_spi_async_collect(int cheetah,int numBytes,byte[] dataIn)
        {
            if (!ChLibraryLoaded) return (int)CheetahStatus.ChIncompatibleLibrary;
            var dataInNumBytes = (int)tp_min(numBytes, dataIn.Length);
            return net_ch_spi_async_collect(cheetah, dataInNumBytes, dataIn);
        }


        /*=========================================================================
        | NATIVE DLL BINDINGS
         ========================================================================*/
        [DllImport(DllFilename)]
        private static extern int net_ch_find_devices(int numDevices, [Out] ushort[] devices);

        [DllImport(DllFilename)]
        private static extern int net_ch_find_devices_ext(int numDevices, [Out] ushort[] devices, int numIds, [Out] uint[] uniqueIds);

        [DllImport(DllFilename)]
        private static extern int net_ch_open(int portNumber);

        [DllImport(DllFilename)]
        private static extern int net_ch_open_ext(int portNumber, ref CheetahExt chExt);

        [DllImport(DllFilename)]
        private static extern int net_ch_close(int cheetah);

        [DllImport(DllFilename)]
        private static extern int net_ch_port(int cheetah);

        [DllImport(DllFilename)]
        private static extern uint net_ch_unique_id(int cheetah);

        [DllImport(DllFilename)]
        private static extern IntPtr net_ch_status_string(int status);

        [DllImport(DllFilename)]
        private static extern int net_ch_version(int cheetah, ref CheetahVersion version);

        [DllImport(DllFilename)]
        private static extern uint net_ch_sleep_ms(uint milliseconds);

        [DllImport(DllFilename)]
        private static extern int net_ch_target_power(int cheetah, byte powerFlag);

        [DllImport(DllFilename)]
        private static extern int net_ch_host_ifce_speed(int cheetah);

        [DllImport(DllFilename)]
        private static extern int net_ch_dev_addr(int cheetah);

        [DllImport(DllFilename)]
        private static extern int net_ch_spi_bitrate(int cheetah, int bitrateKhz);

        [DllImport(DllFilename)]
        private static extern int net_ch_spi_configure(int cheetah, CheetahSpiPolarity polarity, CheetahSpiPhase phase, CheetahSpiBitorder bitorder, byte ssPolarity);

        [DllImport(DllFilename)]
        private static extern int net_ch_spi_queue_clear(int cheetah);

        [DllImport(DllFilename)]
        private static extern int net_ch_spi_queue_oe(int cheetah, byte oe);

        [DllImport(DllFilename)]
        private static extern int net_ch_spi_queue_delay_cycles(int cheetah, int cycles);

        [DllImport(DllFilename)]
        private static extern int net_ch_spi_queue_delay_ns(int cheetah, int nanoseconds);

        [DllImport(DllFilename)]
        private static extern int net_ch_spi_queue_ss(int cheetah, byte active);

        [DllImport(DllFilename)]
        private static extern int net_ch_spi_queue_byte(int cheetah, int count, byte data);

        [DllImport(DllFilename)]
        private static extern int net_ch_spi_queue_array(int cheetah, int numBytes, [In] byte[] dataOut);

        [DllImport(DllFilename)]
        private static extern int net_ch_spi_batch_length(int cheetah);

        [DllImport(DllFilename)]
        private static extern int net_ch_spi_batch_shift(int cheetah, int numBytes, [Out] byte[] dataIn);

        [DllImport(DllFilename)]
        private static extern int net_ch_spi_async_submit(int cheetah);

        [DllImport(DllFilename)]
        private static extern int net_ch_spi_async_collect(int cheetah, int numBytes, [Out] byte[] dataIn);
    }
}
