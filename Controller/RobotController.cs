using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using CommonUtility;

namespace Controller
{
    public sealed class RobotController : ControllerBase
    {
        #region 机器人0状态
        [Description("R,机器人状态_是否IDLE")]
        public string Robot0StateIdle;
        [Description("R,机器人状态_是否StandStill")]
        public string Robot0StateStandStill;
        [Description("R,机器人状态_是否Jogging")]
        public string Robot0StateJogging;
        [Description("R,机器人状态_是否RunBlock")]
        public string Robot0StateRunBlock;
        [Description("R,机器人状态_是否RunProgram")]
        public string Robot0StateRunProgram;
        [Description("R,机器人状态_是否ServoReady")]
        public string Robot0StateServoReady;
        [Description("R,机器人状态_是否Error")]
        public string Robot0StateError;
        [Description("R,机器人状态_全状态")]
        public string Robot0StateAll;
        [Description("R,机器人状态_Error状态")]
        public string Robot0ErrorAll;
        #endregion

        #region 机器人1状态
        [Description("R,机器人状态_是否IDLE")]
        public string Robot1StateIdle;
        [Description("R,机器人状态_是否StandStill")]
        public string Robot1StateStandStill;
        [Description("R,机器人状态_是否Jogging")]
        public string Robot1StateJogging;
        [Description("R,机器人状态_是否RunBlock")]
        public string Robot1StateRunBlock;
        [Description("R,机器人状态_是否RunProgram")]
        public string Robot1StateRunProgram;
        [Description("R,机器人状态_是否ServoReady")]
        public string Robot1StateServoReady;
        [Description("R,机器人状态_是否Error")]
        public string Robot1StateError;
        [Description("R,机器人状态_全状态")]
        public string Robot1StateAll;
        [Description("R,机器人状态_Error状态")]
        public string Robot1ErrorAll;
        #endregion

        #region 机器人2状态
        [Description("R,机器人状态_是否IDLE")]
        public string Robot2StateIdle;
        [Description("R,机器人状态_是否StandStill")]
        public string Robot2StateStandStill;
        [Description("R,机器人状态_是否Jogging")]
        public string Robot2StateJogging;
        [Description("R,机器人状态_是否RunBlock")]
        public string Robot2StateRunBlock;
        [Description("R,机器人状态_是否RunProgram")]
        public string Robot2StateRunProgram;
        [Description("R,机器人状态_是否ServoReady")]
        public string Robot2StateServoReady;
        [Description("R,机器人状态_是否Error")]
        public string Robot2StateError;
        [Description("R,机器人状态_全状态")]
        public string Robot2StateAll;
        [Description("R,机器人状态_Error状态")]
        public string Robot2ErrorAll;
        #endregion

        #region 机器人3状态
        [Description("R,机器人状态_是否IDLE")]
        public string Robot3StateIdle;
        [Description("R,机器人状态_是否StandStill")]
        public string Robot3StateStandStill;
        [Description("R,机器人状态_是否Jogging")]
        public string Robot3StateJogging;
        [Description("R,机器人状态_是否RunBlock")]
        public string Robot3StateRunBlock;
        [Description("R,机器人状态_是否RunProgram")]
        public string Robot3StateRunProgram;
        [Description("R,机器人状态_是否ServoReady")]
        public string Robot3StateServoReady;
        [Description("R,机器人状态_是否Error")]
        public string Robot3StateError;
        [Description("R,机器人状态_全状态")]
        public string Robot3StateAll;
        [Description("R,机器人状态_Error状态")]
        public string Robot3ErrorAll;
        #endregion

        #region 0号机器人M码状态
        [Description("R0,M0")]
        public string R0M0;
        [Description("R0,M1")]
        public string R0M1;
        [Description("R0,M2")]
        public string R0M2;
        [Description("R0,M3")]
        public string R0M3;
        [Description("R0,M4")]
        public string R0M4;
        [Description("R0,M5")]
        public string R0M5;
        [Description("R0,M6")]
        public string R0M6;
        [Description("R0,M7")]
        public string R0M7;
        [Description("R0,M8")]
        public string R0M8;
        [Description("R0,M9")]
        public string R0M9;
        [Description("R0,M10")]
        public string R0M10;
        [Description("R0,M11")]
        public string R0M11;
        [Description("R0,M12")]
        public string R0M12;
        [Description("R0,M13")]
        public string R0M13;
        [Description("R0,M14")]
        public string R0M14;
        [Description("R0,M15")]
        public string R0M15;
        [Description("R0,M16")]
        public string R0M16;
        [Description("R0,M17")]
        public string R0M17;
        [Description("R0,M18")]
        public string R0M18;
        [Description("R0,M19")]
        public string R0M19;
        [Description("R0,M20")]
        public string R0M20;
        [Description("R0,M21")]
        public string R0M21;
        [Description("R0,M22")]
        public string R0M22;
        [Description("R0,M23")]
        public string R0M23;
        [Description("R0,M24")]
        public string R0M24;
        [Description("R0,M25")]
        public string R0M25;
        [Description("R0,M26")]
        public string R0M26;
        [Description("R0,M27")]
        public string R0M27;
        [Description("R0,M28")]
        public string R0M28;
        [Description("R0,M29")]
        public string R0M29;
        [Description("R0,M30")]
        public string R0M30;
        [Description("R0,M31")]
        public string R0M31;
        [Description("R0,M32")]
        public string R0M32;
        [Description("R0,M33")]
        public string R0M33;
        [Description("R0,M34")]
        public string R0M34;
        [Description("R0,M35")]
        public string R0M35;
        [Description("R0,M36")]
        public string R0M36;
        [Description("R0,M37")]
        public string R0M37;
        [Description("R0,M38")]
        public string R0M38;
        [Description("R0,M39")]
        public string R0M39;
        [Description("R0,M40")]
        public string R0M40;
        [Description("R0,M41")]
        public string R0M41;
        [Description("R0,M42")]
        public string R0M42;
        [Description("R0,M43")]
        public string R0M43;
        [Description("R0,M44")]
        public string R0M44;
        [Description("R0,M45")]
        public string R0M45;
        [Description("R0,M46")]
        public string R0M46;
        [Description("R0,M47")]
        public string R0M47;
        [Description("R0,M48")]
        public string R0M48;
        [Description("R0,M49")]
        public string R0M49;
        [Description("R0,M50")]
        public string R0M50;
        [Description("R0,M51")]
        public string R0M51;
        [Description("R0,M52")]
        public string R0M52;
        [Description("R0,M53")]
        public string R0M53;
        [Description("R0,M54")]
        public string R0M54;
        [Description("R0,M55")]
        public string R0M55;
        [Description("R0,M56")]
        public string R0M56;
        [Description("R0,M57")]
        public string R0M57;
        [Description("R0,M58")]
        public string R0M58;
        [Description("R0,M59")]
        public string R0M59;
        [Description("R0,M60")]
        public string R0M60;
        [Description("R0,M61")]
        public string R0M61;
        [Description("R0,M62")]
        public string R0M62;
        [Description("R0,M63")]
        public string R0M63;
        [Description("R0,M64")]
        public string R0M64;
        [Description("R0,M65")]
        public string R0M65;
        [Description("R0,M66")]
        public string R0M66;
        [Description("R0,M67")]
        public string R0M67;
        [Description("R0,M68")]
        public string R0M68;
        [Description("R0,M69")]
        public string R0M69;
        [Description("R0,M70")]
        public string R0M70;
        [Description("R0,M71")]
        public string R0M71;
        [Description("R0,M72")]
        public string R0M72;
        [Description("R0,M73")]
        public string R0M73;
        [Description("R0,M74")]
        public string R0M74;
        [Description("R0,M75")]
        public string R0M75;
        [Description("R0,M76")]
        public string R0M76;
        [Description("R0,M77")]
        public string R0M77;
        [Description("R0,M78")]
        public string R0M78;
        [Description("R0,M79")]
        public string R0M79;
        [Description("R0,M80")]
        public string R0M80;
        [Description("R0,M81")]
        public string R0M81;
        [Description("R0,M82")]
        public string R0M82;
        [Description("R0,M83")]
        public string R0M83;
        [Description("R0,M84")]
        public string R0M84;
        [Description("R0,M85")]
        public string R0M85;
        [Description("R0,M86")]
        public string R0M86;
        [Description("R0,M87")]
        public string R0M87;
        [Description("R0,M88")]
        public string R0M88;
        [Description("R0,M89")]
        public string R0M89;
        [Description("R0,M90")]
        public string R0M90;
        [Description("R0,M91")]
        public string R0M91;
        [Description("R0,M92")]
        public string R0M92;
        [Description("R0,M93")]
        public string R0M93;
        [Description("R0,M94")]
        public string R0M94;
        [Description("R0,M95")]
        public string R0M95;
        [Description("R0,M96")]
        public string R0M96;
        [Description("R0,M97")]
        public string R0M97;
        [Description("R0,M98")]
        public string R0M98;
        [Description("R0,M99")]
        public string R0M99;
        #endregion

        #region 1号机器人M码状态
        [Description("R1,M0")]
        public string R1M0;
        [Description("R1,M1")]
        public string R1M1;
        [Description("R1,M2")]
        public string R1M2;
        [Description("R1,M3")]
        public string R1M3;
        [Description("R1,M4")]
        public string R1M4;
        [Description("R1,M5")]
        public string R1M5;
        [Description("R1,M6")]
        public string R1M6;
        [Description("R1,M7")]
        public string R1M7;
        [Description("R1,M8")]
        public string R1M8;
        [Description("R1,M9")]
        public string R1M9;
        [Description("R1,M10")]
        public string R1M10;
        [Description("R1,M11")]
        public string R1M11;
        [Description("R1,M12")]
        public string R1M12;
        [Description("R1,M13")]
        public string R1M13;
        [Description("R1,M14")]
        public string R1M14;
        [Description("R1,M15")]
        public string R1M15;
        [Description("R1,M16")]
        public string R1M16;
        [Description("R1,M17")]
        public string R1M17;
        [Description("R1,M18")]
        public string R1M18;
        [Description("R1,M19")]
        public string R1M19;
        [Description("R1,M20")]
        public string R1M20;
        [Description("R1,M21")]
        public string R1M21;
        [Description("R1,M22")]
        public string R1M22;
        [Description("R1,M23")]
        public string R1M23;
        [Description("R1,M24")]
        public string R1M24;
        [Description("R1,M25")]
        public string R1M25;
        [Description("R1,M26")]
        public string R1M26;
        [Description("R1,M27")]
        public string R1M27;
        [Description("R1,M28")]
        public string R1M28;
        [Description("R1,M29")]
        public string R1M29;
        [Description("R1,M30")]
        public string R1M30;
        [Description("R1,M31")]
        public string R1M31;
        [Description("R1,M32")]
        public string R1M32;
        [Description("R1,M33")]
        public string R1M33;
        [Description("R1,M34")]
        public string R1M34;
        [Description("R1,M35")]
        public string R1M35;
        [Description("R1,M36")]
        public string R1M36;
        [Description("R1,M37")]
        public string R1M37;
        [Description("R1,M38")]
        public string R1M38;
        [Description("R1,M39")]
        public string R1M39;
        [Description("R1,M40")]
        public string R1M40;
        [Description("R1,M41")]
        public string R1M41;
        [Description("R1,M42")]
        public string R1M42;
        [Description("R1,M43")]
        public string R1M43;
        [Description("R1,M44")]
        public string R1M44;
        [Description("R1,M45")]
        public string R1M45;
        [Description("R1,M46")]
        public string R1M46;
        [Description("R1,M47")]
        public string R1M47;
        [Description("R1,M48")]
        public string R1M48;
        [Description("R1,M49")]
        public string R1M49;
        [Description("R1,M50")]
        public string R1M50;
        [Description("R1,M51")]
        public string R1M51;
        [Description("R1,M52")]
        public string R1M52;
        [Description("R1,M53")]
        public string R1M53;
        [Description("R1,M54")]
        public string R1M54;
        [Description("R1,M55")]
        public string R1M55;
        [Description("R1,M56")]
        public string R1M56;
        [Description("R1,M57")]
        public string R1M57;
        [Description("R1,M58")]
        public string R1M58;
        [Description("R1,M59")]
        public string R1M59;
        [Description("R1,M60")]
        public string R1M60;
        [Description("R1,M61")]
        public string R1M61;
        [Description("R1,M62")]
        public string R1M62;
        [Description("R1,M63")]
        public string R1M63;
        [Description("R1,M64")]
        public string R1M64;
        [Description("R1,M65")]
        public string R1M65;
        [Description("R1,M66")]
        public string R1M66;
        [Description("R1,M67")]
        public string R1M67;
        [Description("R1,M68")]
        public string R1M68;
        [Description("R1,M69")]
        public string R1M69;
        [Description("R1,M70")]
        public string R1M70;
        [Description("R1,M71")]
        public string R1M71;
        [Description("R1,M72")]
        public string R1M72;
        [Description("R1,M73")]
        public string R1M73;
        [Description("R1,M74")]
        public string R1M74;
        [Description("R1,M75")]
        public string R1M75;
        [Description("R1,M76")]
        public string R1M76;
        [Description("R1,M77")]
        public string R1M77;
        [Description("R1,M78")]
        public string R1M78;
        [Description("R1,M79")]
        public string R1M79;
        [Description("R1,M80")]
        public string R1M80;
        [Description("R1,M81")]
        public string R1M81;
        [Description("R1,M82")]
        public string R1M82;
        [Description("R1,M83")]
        public string R1M83;
        [Description("R1,M84")]
        public string R1M84;
        [Description("R1,M85")]
        public string R1M85;
        [Description("R1,M86")]
        public string R1M86;
        [Description("R1,M87")]
        public string R1M87;
        [Description("R1,M88")]
        public string R1M88;
        [Description("R1,M89")]
        public string R1M89;
        [Description("R1,M90")]
        public string R1M90;
        [Description("R1,M91")]
        public string R1M91;
        [Description("R1,M92")]
        public string R1M92;
        [Description("R1,M93")]
        public string R1M93;
        [Description("R1,M94")]
        public string R1M94;
        [Description("R1,M95")]
        public string R1M95;
        [Description("R1,M96")]
        public string R1M96;
        [Description("R1,M97")]
        public string R1M97;
        [Description("R1,M98")]
        public string R1M98;
        [Description("R1,M99")]
        public string R1M99;
        #endregion

        #region 2号机器人M码状态
        [Description("R2,M0")]
        public string R2M0;
        [Description("R2,M1")]
        public string R2M1;
        [Description("R2,M2")]
        public string R2M2;
        [Description("R2,M3")]
        public string R2M3;
        [Description("R2,M4")]
        public string R2M4;
        [Description("R2,M5")]
        public string R2M5;
        [Description("R2,M6")]
        public string R2M6;
        [Description("R2,M7")]
        public string R2M7;
        [Description("R2,M8")]
        public string R2M8;
        [Description("R2,M9")]
        public string R2M9;
        [Description("R2,M10")]
        public string R2M10;
        [Description("R2,M11")]
        public string R2M11;
        [Description("R2,M12")]
        public string R2M12;
        [Description("R2,M13")]
        public string R2M13;
        [Description("R2,M14")]
        public string R2M14;
        [Description("R2,M15")]
        public string R2M15;
        [Description("R2,M16")]
        public string R2M16;
        [Description("R2,M17")]
        public string R2M17;
        [Description("R2,M18")]
        public string R2M18;
        [Description("R2,M19")]
        public string R2M19;
        [Description("R2,M20")]
        public string R2M20;
        [Description("R2,M21")]
        public string R2M21;
        [Description("R2,M22")]
        public string R2M22;
        [Description("R2,M23")]
        public string R2M23;
        [Description("R2,M24")]
        public string R2M24;
        [Description("R2,M25")]
        public string R2M25;
        [Description("R2,M26")]
        public string R2M26;
        [Description("R2,M27")]
        public string R2M27;
        [Description("R2,M28")]
        public string R2M28;
        [Description("R2,M29")]
        public string R2M29;
        [Description("R2,M30")]
        public string R2M30;
        [Description("R2,M31")]
        public string R2M31;
        [Description("R2,M32")]
        public string R2M32;
        [Description("R2,M33")]
        public string R2M33;
        [Description("R2,M34")]
        public string R2M34;
        [Description("R2,M35")]
        public string R2M35;
        [Description("R2,M36")]
        public string R2M36;
        [Description("R2,M37")]
        public string R2M37;
        [Description("R2,M38")]
        public string R2M38;
        [Description("R2,M39")]
        public string R2M39;
        [Description("R2,M40")]
        public string R2M40;
        [Description("R2,M41")]
        public string R2M41;
        [Description("R2,M42")]
        public string R2M42;
        [Description("R2,M43")]
        public string R2M43;
        [Description("R2,M44")]
        public string R2M44;
        [Description("R2,M45")]
        public string R2M45;
        [Description("R2,M46")]
        public string R2M46;
        [Description("R2,M47")]
        public string R2M47;
        [Description("R2,M48")]
        public string R2M48;
        [Description("R2,M49")]
        public string R2M49;
        [Description("R2,M50")]
        public string R2M50;
        [Description("R2,M51")]
        public string R2M51;
        [Description("R2,M52")]
        public string R2M52;
        [Description("R2,M53")]
        public string R2M53;
        [Description("R2,M54")]
        public string R2M54;
        [Description("R2,M55")]
        public string R2M55;
        [Description("R2,M56")]
        public string R2M56;
        [Description("R2,M57")]
        public string R2M57;
        [Description("R2,M58")]
        public string R2M58;
        [Description("R2,M59")]
        public string R2M59;
        [Description("R2,M60")]
        public string R2M60;
        [Description("R2,M61")]
        public string R2M61;
        [Description("R2,M62")]
        public string R2M62;
        [Description("R2,M63")]
        public string R2M63;
        [Description("R2,M64")]
        public string R2M64;
        [Description("R2,M65")]
        public string R2M65;
        [Description("R2,M66")]
        public string R2M66;
        [Description("R2,M67")]
        public string R2M67;
        [Description("R2,M68")]
        public string R2M68;
        [Description("R2,M69")]
        public string R2M69;
        [Description("R2,M70")]
        public string R2M70;
        [Description("R2,M71")]
        public string R2M71;
        [Description("R2,M72")]
        public string R2M72;
        [Description("R2,M73")]
        public string R2M73;
        [Description("R2,M74")]
        public string R2M74;
        [Description("R2,M75")]
        public string R2M75;
        [Description("R2,M76")]
        public string R2M76;
        [Description("R2,M77")]
        public string R2M77;
        [Description("R2,M78")]
        public string R2M78;
        [Description("R2,M79")]
        public string R2M79;
        [Description("R2,M80")]
        public string R2M80;
        [Description("R2,M81")]
        public string R2M81;
        [Description("R2,M82")]
        public string R2M82;
        [Description("R2,M83")]
        public string R2M83;
        [Description("R2,M84")]
        public string R2M84;
        [Description("R2,M85")]
        public string R2M85;
        [Description("R2,M86")]
        public string R2M86;
        [Description("R2,M87")]
        public string R2M87;
        [Description("R2,M88")]
        public string R2M88;
        [Description("R2,M89")]
        public string R2M89;
        [Description("R2,M90")]
        public string R2M90;
        [Description("R2,M91")]
        public string R2M91;
        [Description("R2,M92")]
        public string R2M92;
        [Description("R2,M93")]
        public string R2M93;
        [Description("R2,M94")]
        public string R2M94;
        [Description("R2,M95")]
        public string R2M95;
        [Description("R2,M96")]
        public string R2M96;
        [Description("R2,M97")]
        public string R2M97;
        [Description("R2,M98")]
        public string R2M98;
        [Description("R2,M99")]
        public string R2M99;
        #endregion

        #region 3号机器人M码状态
        [Description("R3,M0")]
        public string R3M0;
        [Description("R3,M1")]
        public string R3M1;
        [Description("R3,M2")]
        public string R3M2;
        [Description("R3,M3")]
        public string R3M3;
        [Description("R3,M4")]
        public string R3M4;
        [Description("R3,M5")]
        public string R3M5;
        [Description("R3,M6")]
        public string R3M6;
        [Description("R3,M7")]
        public string R3M7;
        [Description("R3,M8")]
        public string R3M8;
        [Description("R3,M9")]
        public string R3M9;
        [Description("R3,M10")]
        public string R3M10;
        [Description("R3,M11")]
        public string R3M11;
        [Description("R3,M12")]
        public string R3M12;
        [Description("R3,M13")]
        public string R3M13;
        [Description("R3,M14")]
        public string R3M14;
        [Description("R3,M15")]
        public string R3M15;
        [Description("R3,M16")]
        public string R3M16;
        [Description("R3,M17")]
        public string R3M17;
        [Description("R3,M18")]
        public string R3M18;
        [Description("R3,M19")]
        public string R3M19;
        [Description("R3,M20")]
        public string R3M20;
        [Description("R3,M21")]
        public string R3M21;
        [Description("R3,M22")]
        public string R3M22;
        [Description("R3,M23")]
        public string R3M23;
        [Description("R3,M24")]
        public string R3M24;
        [Description("R3,M25")]
        public string R3M25;
        [Description("R3,M26")]
        public string R3M26;
        [Description("R3,M27")]
        public string R3M27;
        [Description("R3,M28")]
        public string R3M28;
        [Description("R3,M29")]
        public string R3M29;
        [Description("R3,M30")]
        public string R3M30;
        [Description("R3,M31")]
        public string R3M31;
        [Description("R3,M32")]
        public string R3M32;
        [Description("R3,M33")]
        public string R3M33;
        [Description("R3,M34")]
        public string R3M34;
        [Description("R3,M35")]
        public string R3M35;
        [Description("R3,M36")]
        public string R3M36;
        [Description("R3,M37")]
        public string R3M37;
        [Description("R3,M38")]
        public string R3M38;
        [Description("R3,M39")]
        public string R3M39;
        [Description("R3,M40")]
        public string R3M40;
        [Description("R3,M41")]
        public string R3M41;
        [Description("R3,M42")]
        public string R3M42;
        [Description("R3,M43")]
        public string R3M43;
        [Description("R3,M44")]
        public string R3M44;
        [Description("R3,M45")]
        public string R3M45;
        [Description("R3,M46")]
        public string R3M46;
        [Description("R3,M47")]
        public string R3M47;
        [Description("R3,M48")]
        public string R3M48;
        [Description("R3,M49")]
        public string R3M49;
        [Description("R3,M50")]
        public string R3M50;
        [Description("R3,M51")]
        public string R3M51;
        [Description("R3,M52")]
        public string R3M52;
        [Description("R3,M53")]
        public string R3M53;
        [Description("R3,M54")]
        public string R3M54;
        [Description("R3,M55")]
        public string R3M55;
        [Description("R3,M56")]
        public string R3M56;
        [Description("R3,M57")]
        public string R3M57;
        [Description("R3,M58")]
        public string R3M58;
        [Description("R3,M59")]
        public string R3M59;
        [Description("R3,M60")]
        public string R3M60;
        [Description("R3,M61")]
        public string R3M61;
        [Description("R3,M62")]
        public string R3M62;
        [Description("R3,M63")]
        public string R3M63;
        [Description("R3,M64")]
        public string R3M64;
        [Description("R3,M65")]
        public string R3M65;
        [Description("R3,M66")]
        public string R3M66;
        [Description("R3,M67")]
        public string R3M67;
        [Description("R3,M68")]
        public string R3M68;
        [Description("R3,M69")]
        public string R3M69;
        [Description("R3,M70")]
        public string R3M70;
        [Description("R3,M71")]
        public string R3M71;
        [Description("R3,M72")]
        public string R3M72;
        [Description("R3,M73")]
        public string R3M73;
        [Description("R3,M74")]
        public string R3M74;
        [Description("R3,M75")]
        public string R3M75;
        [Description("R3,M76")]
        public string R3M76;
        [Description("R3,M77")]
        public string R3M77;
        [Description("R3,M78")]
        public string R3M78;
        [Description("R3,M79")]
        public string R3M79;
        [Description("R3,M80")]
        public string R3M80;
        [Description("R3,M81")]
        public string R3M81;
        [Description("R3,M82")]
        public string R3M82;
        [Description("R3,M83")]
        public string R3M83;
        [Description("R3,M84")]
        public string R3M84;
        [Description("R3,M85")]
        public string R3M85;
        [Description("R3,M86")]
        public string R3M86;
        [Description("R3,M87")]
        public string R3M87;
        [Description("R3,M88")]
        public string R3M88;
        [Description("R3,M89")]
        public string R3M89;
        [Description("R3,M90")]
        public string R3M90;
        [Description("R3,M91")]
        public string R3M91;
        [Description("R3,M92")]
        public string R3M92;
        [Description("R3,M93")]
        public string R3M93;
        [Description("R3,M94")]
        public string R3M94;
        [Description("R3,M95")]
        public string R3M95;
        [Description("R3,M96")]
        public string R3M96;
        [Description("R3,M97")]
        public string R3M97;
        [Description("R3,M98")]
        public string R3M98;
        [Description("R3,M99")]
        public string R3M99;
        [Description("R3,M70抓取完成")]
        public string R3M70Finished;
        #endregion

        #region DI&Do
        [Description("R,输入_DI0")]
        public string IoInputDi0;
        [Description("R,输入_DI1")]
        public string IoInputDi1;
        [Description("R,输入_DI2")]
        public string IoInputDi2;
        [Description("R,输入_DI3")]
        public string IoInputDi3;
        [Description("R,输入_DI4")]
        public string IoInputDi4;
        [Description("R,输入_DI5")]
        public string IoInputDi5;
        [Description("R,输入_DI6")]
        public string IoInputDi6;
        [Description("R,输入_DI7")]
        public string IoInputDi7;
        [Description("R,输入_DI8")]
        public string IoInputDi8;
        [Description("R,输入_DI9")]
        public string IoInputDi9;
        [Description("R,输入_DI10")]
        public string IoInputDi10;
        [Description("R,输入_DI11")]
        public string IoInputDi11;

        [Description("R/W,继电器_DO0")]
        public string IoOutputDo0 = 0.ToString();
        [Description("R/W,继电器_DO1")]
        public string IoOutputDo1 = 0.ToString();
        [Description("R/W,继电器_DO2")]
        public string IoOutputDo2 = 0.ToString();
        [Description("R/W,继电器_DO3")]
        public string IoOutputDo3 = 0.ToString();

        [Description("R/W,低边输出_DO4")]
        public string IoOutputDo4 = 0.ToString();
        [Description("R/W,低边输出_DO5")]
        public string IoOutputDo5 = 0.ToString();
        [Description("R/W,低边输出_DO6")]
        public string IoOutputDo6 = 0.ToString();
        [Description("R/W,低边输出_DO7")]
        public string IoOutputDo7 = 0.ToString();
        #endregion

        public byte functionCode;
        public byte onlyCode;
        public List<byte> buffLst;
        public EventWaitHandle WaitHandle { get; set; }

        public RobotController(string name)
            : base(name)
        {
            WaitHandle = new AutoResetEvent(false);
        }

        ~RobotController()
        {
            Dispose();
        }

        #region 私有变量&属性
        private MyUdpClient MyUdpClient { get; set; }
        public string RemoteIpPort { get; set; }
        private IPAddress RemoteIpAddress { get; set; }
        private int RemotePort { get; set; }
        private Thread DaemonTh { get; set; }
        public bool IsConnected { get; set; }
        public object ControllerLocker { get; set; }

        private readonly Dictionary<string, List<Matrix>> _matrixDictionary =
            new Dictionary<string, List<Matrix>>();
        #endregion

        #region 公共函数

        public void InitRemoteIpAddress(string ipPort)
        {
            RemoteIpPort = ipPort;

            try
            {
                var sp = ipPort.Split(':');
                var ipAddrStr = sp[0];
                var port = int.Parse(sp[1]);
                RemotePort = port;

                RemoteIpAddress = IPAddress.Parse(ipAddrStr);

                MyUdpClient = ipAddrStr.Equals("127.0.0.1")
                    ? new MyUdpClient("127.0.0.1", port + 1)
                    : new MyUdpClient("192.168.1.50", port);

                MyUdpClient.PushMsgEvent += _myUdpClient_PushMsgEvent;
                MyUdpClient.AddRemoteClient(ipAddrStr, port);
                MyUdpClient.BeginReceive();
                IsConnected = true;
                ControllerLocker = new object();

                if (DaemonTh != null)
                {
                    DaemonTh.Abort();
                    DaemonTh.Join();
                }

                DaemonTh = new Thread(Daemon) { IsBackground = true };
                DaemonTh.Start();
            }
            catch (Exception)
            {
                IsConnected = false;
            }
        }

        [Description("运行程序")]
        public void RunProgram(string robotID, string programName)
        {
            if (programName.Length < 33)
            {
                //00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F 10 11 12 13 14 15 16 17 18 19 1A 1B 1C 1D 1E 1F(program name32bytes)
                List<byte> sendBytes = new List<byte>(new byte[] { 0x00, 0x01, 0x00, 0x00, 0x00, 0x28, 0x01, 0x67, 0x00, 0x00, 0x00, 0x00, 0x21 });
                sendBytes.Add(byte.Parse(robotID));
                sendBytes.AddRange(Encoding.ASCII.GetBytes(programName));

                for (int i = 0; i < 32 - programName.Length; i++)
                {
                    sendBytes.Add(0);
                }
                SendMsg(sendBytes.ToArray());
            }
        }

        [Description("使能ON")]
        public void ServoOn(string robotID)
        {
            List<byte> sendBytes = new List<byte>(new byte[] { 0x00, 0x01, 0x00, 0x00, 0x00, 0x09 });
            sendBytes.Add(0x01);//modbus id
            sendBytes.Add(0x68);//function code
            sendBytes.AddRange((new byte[] { 0, 0, 0, 0, 2 }));
            sendBytes.Add(byte.Parse(robotID));//robotId
            sendBytes.Add(0x0A);//ServoOn code
            SendMsg(sendBytes.ToArray());
        }

        [Description("使能OFF")]
        public void ServoOff(string robotID)
        {
            List<byte> sendBytes = new List<byte>(new byte[] { 0x00, 0x01, 0x00, 0x00, 0x00, 0x09 });
            sendBytes.Add(0x01);//modbus id
            sendBytes.Add(0x68);//function code
            sendBytes.AddRange((new byte[] { 0, 0, 0, 0, 2 }));
            sendBytes.Add(byte.Parse(robotID));//robotId
            sendBytes.Add(0x09);//ServoOff code
            SendMsg(sendBytes.ToArray());
        }

        [Description("机器人重置")]
        public void RobotReset()
        {
            List<byte> sendBytes = new List<byte>(new byte[] { 0x00, 0x01, 0x00, 0x00, 0x00, 0x09, 0x01, 0x75, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x11 });
            SendMsg(sendBytes.ToArray());
        }

        [Description("M码重置")]
        public void ResetMCode(string robotID, string mCode)
        {
            if (mCode.Substring(0, 3) == "R" + robotID + "M")
            {
                var field = GetType().GetField(mCode);
                if (field == null)
                    return;
                List<byte> sendBytes = new List<byte>(new byte[] { 0x00, 0x01, 0x00, 0x00, 0x00, 0x0C });
                sendBytes.Add(0x01);//modbus id
                sendBytes.Add(0x74);//(function code
                sendBytes.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x00 });
                sendBytes.Add(0x04);
                sendBytes.Add(byte.Parse(robotID));//robotId
                sendBytes.Add(0x02);
                sendBytes.Add(Convert.ToByte(mCode.Substring(3, mCode.Length - 3)));//MS I名
                sendBytes.Add(0x00);//0表重置当前MS I……的M码
                SendMsg(sendBytes.ToArray());
                //SendMsg(00 01 00 00 00 0C 01 74 00 00 00 00 04 01 02 39 00)

                while (true)
                {
                    var filedValue = field.GetValue(this).ToString();
                    if (filedValue == "0")
                        break;
                }
            }
        }


        [Description("发送获取板子的DI、DO值的报文")]
        private void SendDIDO()
        {
            List<byte> sendBytes = new List<byte>(new byte[] { 0x00, 0x01, 0x00, 0x00, 0x00, 0x06, 0x01, 0x03, 0x00, 0x11, 0x00, 0x1C });
            SendMsg(sendBytes.ToArray());
        }

        [Description("发送获取各机器人运行状态的报文")]
        private void SendRobotState(byte RobID)
        {
            List<byte> sendBytes = new List<byte>(new byte[] { 0x00, 0x01, 0x00, 0x00, 0x00, 0x06, 0x01, 0x03 });

            sendBytes.Add(RobID);

            sendBytes.AddRange(new byte[] { 0x00, 0x01, 0x80 });
            SendMsg(sendBytes.ToArray());
        }

        [Description("添加一个矩阵")]
        public void AddMatrix(string code) // MatrixName:program1|program2|program3...
        {
            var sp1 = code.Split(':');
            var matrixName = sp1[0];
            var programs = sp1[1];

            if (_matrixDictionary.ContainsKey(matrixName))
                return;
            _matrixDictionary.Add(matrixName, new List<Matrix>());

            foreach (var t in programs.Split('|').Where(t => !string.IsNullOrEmpty(t)))
                _matrixDictionary[matrixName].Add(new Matrix(t));
        }

        [Description("移除一个矩阵")]
        public void RemoveMaxtrix(string matrixName)
        {
            if (!_matrixDictionary.ContainsKey(matrixName))
                return;
            _matrixDictionary.Remove(matrixName);
        }

        [Description("移除所有矩阵")]
        public void ClearMatrix()
        {
            _matrixDictionary.Clear();
        }

        [Description("运行矩阵")]
        public void RunMatrix(string robotID, string matrixName)
        {
            if (!_matrixDictionary.ContainsKey(matrixName))
                return;

            var programs = _matrixDictionary[matrixName];

            if (programs.FindAll(f => f.IsRun).Count == programs.Count)
                foreach (var p in programs)
                    p.IsRun = false;

            foreach (var p in programs.Where(p => !p.IsRun))
            {
                p.IsRun = true;
                RunProgram(robotID, p.Name);
                return;
            }
        }

        [Description("重置矩阵")]
        public void ResetMatrix(string matrixName)
        {
            if (!_matrixDictionary.ContainsKey(matrixName))
                return;

            var programs = _matrixDictionary[matrixName];

            foreach (var p in programs)
                p.IsRun = false;
        }

        #endregion

        #region 发送与接收

        private void Daemon()
        {
            while (DaemonTh.IsAlive)
            {
                if (!DaemonTh.IsAlive)
                    break;

                if (MyUdpClient == null)
                    continue;

                SendDIDO();
                SendRobotState(0x20);
                //Thread.Sleep(50);
                SendRobotState(0x40);
                //Thread.Sleep(50);
                SendRobotState(0x60);
                //Thread.Sleep(50);
                SendRobotState(0x80);


                // Set_IO (function code 0x10)
                var bs = new byte[]
                    {
                        0x00, 0x01, 0x00, 0x00, 0x00, 0x17, 0x01, 0x10, 0x00, 0x25, 0x00, 0x08, 0x10,
                        0x00, IoOutputDo0 == "1" ? (byte) 0x01 : (byte) 0x00,
                        0x00, IoOutputDo1 == "1" ? (byte) 0x01 : (byte) 0x00,
                        0x00, IoOutputDo2 == "1" ? (byte) 0x01 : (byte) 0x00,
                        0x00, IoOutputDo3 == "1" ? (byte) 0x01 : (byte) 0x00,
                        0x00, IoOutputDo4 == "1" ? (byte) 0x01 : (byte) 0x00,
                        0x00, IoOutputDo5 == "1" ? (byte) 0x01 : (byte) 0x00,
                        0x00, IoOutputDo6 == "1" ? (byte) 0x01 : (byte) 0x00,
                        0x00, IoOutputDo7 == "1" ? (byte) 0x01 : (byte) 0x00,
                    };
                SendMsg(bs);
            }
        }

        private void _myUdpClient_PushMsgEvent(EndPoint ipEndPoint, byte[] bytes)
        {
            if (bytes.Length < 13)
                return;
            buffLst = bytes.ToList();
            //do
            {
                if (buffLst.Count < 12)
                {
                    buffLst.Clear();
                    //continue;
                }
                if (buffLst.Count == 65 || buffLst.Count == 559)
                {
                    functionCode = bytes[7];
                    onlyCode = bytes[8];
                }

            }


            //读取板子DI和DO
            if (functionCode == 0x03 && onlyCode == 0x38)
            {
                IoInputDi0 = buffLst[25].ToString();
                IoInputDi1 = buffLst[27].ToString();
                IoInputDi2 = buffLst[29].ToString();
                IoInputDi3 = buffLst[31].ToString();
                IoInputDi4 = buffLst[33].ToString();
                IoInputDi5 = buffLst[35].ToString();
                IoInputDi6 = buffLst[37].ToString();
                IoInputDi7 = buffLst[39].ToString();
                IoInputDi8 = buffLst[41].ToString();
                IoInputDi9 = buffLst[43].ToString();
                IoInputDi10 = buffLst[45].ToString();
                IoInputDi11 = buffLst[47].ToString();

                WaitHandle.Set();
                //IoOutputDo0 = buffLst[50].ToString();
                //IoOutputDo1 = buffLst[52].ToString();
                //IoOutputDo2 = buffLst[54].ToString();
                //IoOutputDo3 = buffLst[56].ToString();
                //IoOutputDo4 = buffLst[58].ToString();
                //IoOutputDo5 = buffLst[60].ToString();
                //IoOutputDo6 = buffLst[62].ToString();
                //IoOutputDo7 = buffLst[64].ToString();
                //IoOutputDo0 = buffLst[49].ToString();
                //IoOutputDo1 = buffLst[51].ToString();
                //IoOutputDo2 = buffLst[53].ToString();
                //IoOutputDo3 = buffLst[55].ToString();
                //IoOutputDo4 = buffLst[57].ToString();
                //IoOutputDo5 = buffLst[59].ToString();
                //IoOutputDo6 = buffLst[61].ToString();
                //IoOutputDo7 = buffLst[63].ToString();
            }
            // Read_robots_monitor status (function code 0x03)


            //读取0号机器人的运行状态
            else if (functionCode == 0x03 && onlyCode == 0x20)
            {
                Robot0StateAll = buffLst[11].ToString();
                var errorcode = new byte[2];
                Array.Copy(buffLst.ToArray(), 13, errorcode, 0, 2);
                Robot0ErrorAll = BitConverter.ToInt16(errorcode, 0).ToString();
                R0M0 = buffLst[305].ToString();
                R0M1 = buffLst[306].ToString();
                R0M2 = buffLst[307].ToString();
                R0M3 = buffLst[308].ToString();
                R0M4 = buffLst[309].ToString();
                R0M5 = buffLst[310].ToString();
                R0M6 = buffLst[311].ToString();
                R0M7 = buffLst[312].ToString();
                R0M8 = buffLst[313].ToString();
                R0M9 = buffLst[314].ToString();
                R0M10 = buffLst[315].ToString();
                R0M11 = buffLst[316].ToString();
                R0M12 = buffLst[317].ToString();
                R0M13 = buffLst[318].ToString();
                R0M14 = buffLst[319].ToString();
                R0M15 = buffLst[320].ToString();
                R0M16 = buffLst[321].ToString();
                R0M17 = buffLst[322].ToString();
                R0M18 = buffLst[323].ToString();
                R0M19 = buffLst[324].ToString();

                WaitHandle.Set();
                /*switch (Convert.ToInt32(buffLst[11].ToString()))
                {
                    case 01:
                        Robot0StateStandStill = "1";
                        break;
                    case 10:
                        Robot0StateJogging = "1";
                        break;
                    case 20:
                        Robot0StateRunBlock = "1";
                        break;
                    case 30:
                        Robot0StateRunProgram = "1";
                        break;
                    case 40:
                        Robot0StateServoReady = "1";
                        break;
                    case 00:
                        Robot0StateIdle = "1";
                        break;
                    case 255:
                        Robot0StateError = "1";
                        break;
                }*/
            }
            //读取1号机器人的运行状态
            else if (functionCode == 0x03 && onlyCode == 0x40)
            {
                Robot1StateAll = buffLst[11].ToString();
                var errorcode = new byte[2];
                Array.Copy(buffLst.ToArray(), 13, errorcode, 0, 2);
                Robot1ErrorAll = BitConverter.ToInt16(errorcode, 0).ToString();
                R1M0 = buffLst[305].ToString();
                R1M1 = buffLst[306].ToString();
                R1M2 = buffLst[307].ToString();
                R1M3 = buffLst[308].ToString();
                R1M4 = buffLst[309].ToString();
                R1M5 = buffLst[310].ToString();
                R1M6 = buffLst[311].ToString();
                R1M7 = buffLst[312].ToString();
                R1M8 = buffLst[313].ToString();
                R1M9 = buffLst[314].ToString();
                R1M10 = buffLst[315].ToString();
                R1M11 = buffLst[316].ToString();
                R1M12 = buffLst[317].ToString();
                R1M13 = buffLst[318].ToString();
                R1M14 = buffLst[319].ToString();
                R1M15 = buffLst[320].ToString();
                R1M16 = buffLst[321].ToString();
                R1M17 = buffLst[322].ToString();
                R1M18 = buffLst[323].ToString();
                R1M19 = buffLst[324].ToString();

                WaitHandle.Set();
                /*switch (Convert.ToInt32(buffLst[11].ToString()))
                {
                    case 01:
                        Robot1StateStandStill = "1";
                        break;
                    case 10:
                        Robot1StateJogging = "1";
                        break;
                    case 20:
                        Robot1StateRunBlock = "1";
                        break;
                    case 30:
                        Robot1StateRunProgram = "1";
                        break;
                    case 40:
                        Robot1StateServoReady = "1";
                        break;
                    case 00:
                        Robot1StateIdle = "1";
                        break;
                    case 255:
                        Robot1StateError = "1";
                        break;
                }*/
            }
            //读取2号机器人的运行状态
            else if (functionCode == 0x03 && onlyCode == 0x60)
            {
                Robot2StateAll = buffLst[11].ToString();
                var errorcode = new byte[2];
                Array.Copy(buffLst.ToArray(), 13, errorcode, 0, 2);
                Robot2ErrorAll = BitConverter.ToInt16(errorcode, 0).ToString();
                R2M0 = buffLst[305].ToString();
                R2M1 = buffLst[306].ToString();
                R2M2 = buffLst[307].ToString();
                R2M3 = buffLst[308].ToString();
                R2M4 = buffLst[309].ToString();
                R2M5 = buffLst[310].ToString();
                R2M6 = buffLst[311].ToString();
                R2M7 = buffLst[312].ToString();
                R2M8 = buffLst[313].ToString();
                R2M9 = buffLst[314].ToString();
                R2M10 = buffLst[315].ToString();
                R2M11 = buffLst[316].ToString();
                R2M12 = buffLst[317].ToString();
                R2M13 = buffLst[318].ToString();
                R2M14 = buffLst[319].ToString();
                R2M15 = buffLst[320].ToString();
                R2M16 = buffLst[321].ToString();
                R2M17 = buffLst[322].ToString();
                R2M18 = buffLst[323].ToString();
                R2M19 = buffLst[324].ToString();

                WaitHandle.Set();
                /*switch (Convert.ToInt32(buffLst[11].ToString()))
                {
                    case 01:
                        Robot2StateStandStill = "1";
                        break;
                    case 10:
                        Robot2StateJogging = "1";
                        break;
                    case 20:
                        Robot2StateRunBlock = "1";
                        break;
                    case 30:
                        Robot2StateRunProgram = "1";
                        break;
                    case 40:
                        Robot2StateServoReady = "1";
                        break;
                    case 00:
                        Robot2StateIdle = "1";
                        break;
                    case 255:
                        Robot2StateError = "1";
                        break;
                }*/
            }
            //读取3号机器人的运行状态
            else if (functionCode == 0x03 && onlyCode == 0x80)
            {
                Robot3StateAll = buffLst[11].ToString();
                var errorcode = new byte[2];
                Array.Copy(buffLst.ToArray(), 13, errorcode, 0, 2);
                Robot3ErrorAll = BitConverter.ToInt16(errorcode, 0).ToString();
                R3M0 = buffLst[305].ToString();
                R3M1 = buffLst[306].ToString();
                R3M2 = buffLst[307].ToString();
                R3M3 = buffLst[308].ToString();
                R3M4 = buffLst[309].ToString();
                R3M5 = buffLst[310].ToString();
                R3M6 = buffLst[311].ToString();
                R3M7 = buffLst[312].ToString();
                R3M8 = buffLst[313].ToString();
                R3M9 = buffLst[314].ToString();
                R3M10 = buffLst[315].ToString();
                R3M11 = buffLst[316].ToString();
                R3M12 = buffLst[317].ToString();
                R3M13 = buffLst[318].ToString();
                R3M14 = buffLst[319].ToString();
                R3M15 = buffLst[320].ToString();
                R3M16 = buffLst[321].ToString();
                R3M17 = buffLst[322].ToString();
                R3M18 = buffLst[323].ToString();
                R3M19 = buffLst[324].ToString();

                WaitHandle.Set();
                /*switch (Convert.ToInt32(buffLst[11].ToString()))
                {
                    case 01:
                        Robot3StateStandStill = "1";
                        break;
                    case 10:
                        Robot3StateJogging = "1";
                        break;
                    case 20:
                        Robot3StateRunBlock = "1";
                        break;
                    case 30:
                        Robot3StateRunProgram = "1";
                        break;
                    case 40:
                        Robot3StateServoReady = "1";
                        break;
                    case 00:
                        Robot3StateIdle = "1";
                        break;
                    case 255:
                        Robot3StateError = "1";
                        break;
                }
                R3M70Finished= buffLst[325].ToString();*/
            }
        }

        private void SendMsg(byte[] bytes)
        {
            if (MyUdpClient == null)
                return;
            lock (ControllerLocker)
            {
                MyUdpClient.SendMsgTo(new IPEndPoint(RemoteIpAddress, RemotePort), bytes);
                WaitHandle.WaitOne(500);
            }
        }

        #endregion

        internal class Matrix
        {
            public string Name;
            public bool IsRun;

            public Matrix(string name)
            {
                Name = name;
            }
        }
    }
}
