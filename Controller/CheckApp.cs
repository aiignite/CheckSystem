using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using CommonUtility;

namespace Controller
{
    public sealed class CheckApp : ControllerBase
    {
        [Description("R/W,系统变量IsByPass")]
        public bool IsByPass;

        [Description("R/W,系统变量IsRun")]
        public bool IsRun;

        [Description("R,YesOrNo")]
        public string YesOrNo;

        //public bool IsManualExecute;
        public bool EqualTrue = true;

        #region Bool型变量
        [Description("R/W,系统变量Bbool0")]
        public bool Bbool0;
        [Description("R/W,系统变量Bbool1")]
        public bool Bbool1;
        [Description("R/W,系统变量Bbool2")]
        public bool Bbool2;
        [Description("R/W,系统变量Bbool3")]
        public bool Bbool3;
        [Description("R/W,系统变量Bbool4")]
        public bool Bbool4;
        [Description("R/W,系统变量Bbool5")]
        public bool Bbool5;
        [Description("R/W,系统变量Bbool6")]
        public bool Bbool6;
        [Description("R/W,系统变量Bbool7")]
        public bool Bbool7;
        [Description("R/W,系统变量Bbool8")]
        public bool Bbool8;
        [Description("R/W,系统变量Bbool9")]
        public bool Bbool9;
        [Description("R/W,系统变量Bbool10")]
        public bool Bbool10;
        [Description("R/W,系统变量Bbool11")]
        public bool Bbool11;
        [Description("R/W,系统变量Bbool12")]
        public bool Bbool12;
        [Description("R/W,系统变量Bbool13")]
        public bool Bbool13;
        [Description("R/W,系统变量Bbool14")]
        public bool Bbool14;
        [Description("R/W,系统变量Bbool15")]
        public bool Bbool15;
        [Description("R/W,系统变量Bbool16")]
        public bool Bbool16;
        [Description("R/W,系统变量Bbool17")]
        public bool Bbool17;
        [Description("R/W,系统变量Bbool18")]
        public bool Bbool18;
        [Description("R/W,系统变量Bbool19")]
        public bool Bbool19;
        [Description("R/W,系统变量Bbool20")]
        public bool Bbool20;
        [Description("R/W,系统变量Bbool21")]
        public bool Bbool21;
        [Description("R/W,系统变量Bbool22")]
        public bool Bbool22;
        [Description("R/W,系统变量Bbool23")]
        public bool Bbool23;
        [Description("R/W,系统变量Bbool24")]
        public bool Bbool24;
        [Description("R/W,系统变量Bbool25")]
        public bool Bbool25;
        [Description("R/W,系统变量Bbool26")]
        public bool Bbool26;
        [Description("R/W,系统变量Bbool27")]
        public bool Bbool27;
        [Description("R/W,系统变量Bbool28")]
        public bool Bbool28;
        [Description("R/W,系统变量Bbool29")]
        public bool Bbool29;
        [Description("R/W,系统变量Bbool30")]
        public bool Bbool30;
        [Description("R/W,系统变量Bbool31")]
        public bool Bbool31;
        [Description("R/W,系统变量Bbool32")]
        public bool Bbool32;
        [Description("R/W,系统变量Bbool33")]
        public bool Bbool33;
        [Description("R/W,系统变量Bbool34")]
        public bool Bbool34;
        [Description("R/W,系统变量Bbool35")]
        public bool Bbool35;
        [Description("R/W,系统变量Bbool36")]
        public bool Bbool36;
        [Description("R/W,系统变量Bbool37")]
        public bool Bbool37;
        [Description("R/W,系统变量Bbool38")]
        public bool Bbool38;
        [Description("R/W,系统变量Bbool39")]
        public bool Bbool39;
        [Description("R/W,系统变量Bbool40")]
        public bool Bbool40;
        [Description("R/W,系统变量Bbool41")]
        public bool Bbool41;
        [Description("R/W,系统变量Bbool42")]
        public bool Bbool42;
        [Description("R/W,系统变量Bbool43")]
        public bool Bbool43;
        [Description("R/W,系统变量Bbool44")]
        public bool Bbool44;
        [Description("R/W,系统变量Bbool45")]
        public bool Bbool45;
        [Description("R/W,系统变量Bbool46")]
        public bool Bbool46;
        [Description("R/W,系统变量Bbool47")]
        public bool Bbool47;
        [Description("R/W,系统变量Bbool48")]
        public bool Bbool48;
        [Description("R/W,系统变量Bbool49")]
        public bool Bbool49;
        [Description("R/W,系统变量Bbool50")]
        public bool Bbool50;
        [Description("R/W,系统变量Bbool51")]
        public bool Bbool51;
        [Description("R/W,系统变量Bbool52")]
        public bool Bbool52;
        [Description("R/W,系统变量Bbool53")]
        public bool Bbool53;
        [Description("R/W,系统变量Bbool54")]
        public bool Bbool54;
        [Description("R/W,系统变量Bbool55")]
        public bool Bbool55;
        [Description("R/W,系统变量Bbool56")]
        public bool Bbool56;
        [Description("R/W,系统变量Bbool57")]
        public bool Bbool57;
        [Description("R/W,系统变量Bbool58")]
        public bool Bbool58;
        [Description("R/W,系统变量Bbool59")]
        public bool Bbool59;
        [Description("R/W,系统变量Bbool60")]
        public bool Bbool60;
        [Description("R/W,系统变量Bbool61")]
        public bool Bbool61;
        [Description("R/W,系统变量Bbool62")]
        public bool Bbool62;
        [Description("R/W,系统变量Bbool63")]
        public bool Bbool63;
        [Description("R/W,系统变量Bbool64")]
        public bool Bbool64;
        [Description("R/W,系统变量Bbool65")]
        public bool Bbool65;
        [Description("R/W,系统变量Bbool66")]
        public bool Bbool66;
        [Description("R/W,系统变量Bbool67")]
        public bool Bbool67;
        [Description("R/W,系统变量Bbool68")]
        public bool Bbool68;
        [Description("R/W,系统变量Bbool69")]
        public bool Bbool69;
        [Description("R/W,系统变量Bbool70")]
        public bool Bbool70;
        [Description("R/W,系统变量Bbool71")]
        public bool Bbool71;
        [Description("R/W,系统变量Bbool72")]
        public bool Bbool72;
        [Description("R/W,系统变量Bbool73")]
        public bool Bbool73;
        [Description("R/W,系统变量Bbool74")]
        public bool Bbool74;
        [Description("R/W,系统变量Bbool75")]
        public bool Bbool75;
        [Description("R/W,系统变量Bbool76")]
        public bool Bbool76;
        [Description("R/W,系统变量Bbool77")]
        public bool Bbool77;
        [Description("R/W,系统变量Bbool78")]
        public bool Bbool78;
        [Description("R/W,系统变量Bbool79")]
        public bool Bbool79;
        [Description("R/W,系统变量Bbool80")]
        public bool Bbool80;
        [Description("R/W,系统变量Bbool81")]
        public bool Bbool81;
        [Description("R/W,系统变量Bbool82")]
        public bool Bbool82;
        [Description("R/W,系统变量Bbool83")]
        public bool Bbool83;
        [Description("R/W,系统变量Bbool84")]
        public bool Bbool84;
        [Description("R/W,系统变量Bbool85")]
        public bool Bbool85;
        [Description("R/W,系统变量Bbool86")]
        public bool Bbool86;
        [Description("R/W,系统变量Bbool87")]
        public bool Bbool87;
        [Description("R/W,系统变量Bbool88")]
        public bool Bbool88;
        [Description("R/W,系统变量Bbool89")]
        public bool Bbool89;
        [Description("R/W,系统变量Bbool90")]
        public bool Bbool90;
        [Description("R/W,系统变量Bbool91")]
        public bool Bbool91;
        [Description("R/W,系统变量Bbool92")]
        public bool Bbool92;
        [Description("R/W,系统变量Bbool93")]
        public bool Bbool93;
        [Description("R/W,系统变量Bbool94")]
        public bool Bbool94;
        [Description("R/W,系统变量Bbool95")]
        public bool Bbool95;
        [Description("R/W,系统变量Bbool96")]
        public bool Bbool96;
        [Description("R/W,系统变量Bbool97")]
        public bool Bbool97;
        [Description("R/W,系统变量Bbool98")]
        public bool Bbool98;
        [Description("R/W,系统变量Bbool99")]
        public bool Bbool99;
        [Description("R/W,系统变量Bbool10")]
        public bool Bbool100;
        [Description("R/W,系统变量Bbool11")]
        public bool Bbool101;
        [Description("R/W,系统变量Bbool12")]
        public bool Bbool102;
        [Description("R/W,系统变量Bbool13")]
        public bool Bbool103;
        [Description("R/W,系统变量Bbool14")]
        public bool Bbool104;
        [Description("R/W,系统变量Bbool15")]
        public bool Bbool105;
        [Description("R/W,系统变量Bbool16")]
        public bool Bbool106;
        [Description("R/W,系统变量Bbool17")]
        public bool Bbool107;
        [Description("R/W,系统变量Bbool18")]
        public bool Bbool108;
        [Description("R/W,系统变量Bbool19")]
        public bool Bbool109;
        [Description("R/W,系统变量Bbool110")]
        public bool Bbool110;
        [Description("R/W,系统变量Bbool111")]
        public bool Bbool111;
        [Description("R/W,系统变量Bbool112")]
        public bool Bbool112;
        [Description("R/W,系统变量Bbool113")]
        public bool Bbool113;
        [Description("R/W,系统变量Bbool114")]
        public bool Bbool114;
        [Description("R/W,系统变量Bbool115")]
        public bool Bbool115;
        [Description("R/W,系统变量Bbool116")]
        public bool Bbool116;
        [Description("R/W,系统变量Bbool117")]
        public bool Bbool117;
        [Description("R/W,系统变量Bbool118")]
        public bool Bbool118;
        [Description("R/W,系统变量Bbool119")]
        public bool Bbool119;
        [Description("R/W,系统变量Bbool120")]
        public bool Bbool120;
        [Description("R/W,系统变量Bbool121")]
        public bool Bbool121;
        [Description("R/W,系统变量Bbool122")]
        public bool Bbool122;
        [Description("R/W,系统变量Bbool123")]
        public bool Bbool123;
        [Description("R/W,系统变量Bbool124")]
        public bool Bbool124;
        [Description("R/W,系统变量Bbool125")]
        public bool Bbool125;
        [Description("R/W,系统变量Bbool126")]
        public bool Bbool126;
        [Description("R/W,系统变量Bbool127")]
        public bool Bbool127;
        [Description("R/W,系统变量Bbool128")]
        public bool Bbool128;
        [Description("R/W,系统变量Bbool129")]
        public bool Bbool129;
        [Description("R/W,系统变量Bbool130")]
        public bool Bbool130;
        [Description("R/W,系统变量Bbool131")]
        public bool Bbool131;
        [Description("R/W,系统变量Bbool132")]
        public bool Bbool132;
        [Description("R/W,系统变量Bbool133")]
        public bool Bbool133;
        [Description("R/W,系统变量Bbool134")]
        public bool Bbool134;
        [Description("R/W,系统变量Bbool135")]
        public bool Bbool135;
        [Description("R/W,系统变量Bbool136")]
        public bool Bbool136;
        [Description("R/W,系统变量Bbool137")]
        public bool Bbool137;
        [Description("R/W,系统变量Bbool138")]
        public bool Bbool138;
        [Description("R/W,系统变量Bbool139")]
        public bool Bbool139;
        [Description("R/W,系统变量Bbool140")]
        public bool Bbool140;
        [Description("R/W,系统变量Bbool141")]
        public bool Bbool141;
        [Description("R/W,系统变量Bbool142")]
        public bool Bbool142;
        [Description("R/W,系统变量Bbool143")]
        public bool Bbool143;
        [Description("R/W,系统变量Bbool144")]
        public bool Bbool144;
        [Description("R/W,系统变量Bbool145")]
        public bool Bbool145;
        [Description("R/W,系统变量Bbool146")]
        public bool Bbool146;
        [Description("R/W,系统变量Bbool147")]
        public bool Bbool147;
        [Description("R/W,系统变量Bbool148")]
        public bool Bbool148;
        [Description("R/W,系统变量Bbool149")]
        public bool Bbool149;
        [Description("R/W,系统变量Bbool150")]
        public bool Bbool150;
        [Description("R/W,系统变量Bbool151")]
        public bool Bbool151;
        [Description("R/W,系统变量Bbool152")]
        public bool Bbool152;
        [Description("R/W,系统变量Bbool153")]
        public bool Bbool153;
        [Description("R/W,系统变量Bbool154")]
        public bool Bbool154;
        [Description("R/W,系统变量Bbool155")]
        public bool Bbool155;
        [Description("R/W,系统变量Bbool156")]
        public bool Bbool156;
        [Description("R/W,系统变量Bbool157")]
        public bool Bbool157;
        [Description("R/W,系统变量Bbool158")]
        public bool Bbool158;
        [Description("R/W,系统变量Bbool159")]
        public bool Bbool159;
        [Description("R/W,系统变量Bbool160")]
        public bool Bbool160;
        [Description("R/W,系统变量Bbool161")]
        public bool Bbool161;
        [Description("R/W,系统变量Bbool162")]
        public bool Bbool162;
        [Description("R/W,系统变量Bbool163")]
        public bool Bbool163;
        [Description("R/W,系统变量Bbool164")]
        public bool Bbool164;
        [Description("R/W,系统变量Bbool165")]
        public bool Bbool165;
        [Description("R/W,系统变量Bbool166")]
        public bool Bbool166;
        [Description("R/W,系统变量Bbool167")]
        public bool Bbool167;
        [Description("R/W,系统变量Bbool168")]
        public bool Bbool168;
        [Description("R/W,系统变量Bbool169")]
        public bool Bbool169;
        [Description("R/W,系统变量Bbool170")]
        public bool Bbool170;
        [Description("R/W,系统变量Bbool171")]
        public bool Bbool171;
        [Description("R/W,系统变量Bbool172")]
        public bool Bbool172;
        [Description("R/W,系统变量Bbool173")]
        public bool Bbool173;
        [Description("R/W,系统变量Bbool174")]
        public bool Bbool174;
        [Description("R/W,系统变量Bbool175")]
        public bool Bbool175;
        [Description("R/W,系统变量Bbool176")]
        public bool Bbool176;
        [Description("R/W,系统变量Bbool177")]
        public bool Bbool177;
        [Description("R/W,系统变量Bbool178")]
        public bool Bbool178;
        [Description("R/W,系统变量Bbool179")]
        public bool Bbool179;
        [Description("R/W,系统变量Bbool180")]
        public bool Bbool180;
        [Description("R/W,系统变量Bbool181")]
        public bool Bbool181;
        [Description("R/W,系统变量Bbool182")]
        public bool Bbool182;
        [Description("R/W,系统变量Bbool183")]
        public bool Bbool183;
        [Description("R/W,系统变量Bbool184")]
        public bool Bbool184;
        [Description("R/W,系统变量Bbool185")]
        public bool Bbool185;
        [Description("R/W,系统变量Bbool186")]
        public bool Bbool186;
        [Description("R/W,系统变量Bbool187")]
        public bool Bbool187;
        [Description("R/W,系统变量Bbool188")]
        public bool Bbool188;
        [Description("R/W,系统变量Bbool189")]
        public bool Bbool189;
        [Description("R/W,系统变量Bbool190")]
        public bool Bbool190;
        [Description("R/W,系统变量Bbool191")]
        public bool Bbool191;
        [Description("R/W,系统变量Bbool192")]
        public bool Bbool192;
        [Description("R/W,系统变量Bbool193")]
        public bool Bbool193;
        [Description("R/W,系统变量Bbool194")]
        public bool Bbool194;
        [Description("R/W,系统变量Bbool195")]
        public bool Bbool195;
        [Description("R/W,系统变量Bbool196")]
        public bool Bbool196;
        [Description("R/W,系统变量Bbool197")]
        public bool Bbool197;
        [Description("R/W,系统变量Bbool198")]
        public bool Bbool198;
        [Description("R/W,系统变量Bbool199")]
        public bool Bbool199;
        [Description("R/W,系统变量Bbool20")]
        public bool Bbool200;
        [Description("R/W,系统变量Bbool21")]
        public bool Bbool201;
        [Description("R/W,系统变量Bbool22")]
        public bool Bbool202;
        [Description("R/W,系统变量Bbool23")]
        public bool Bbool203;
        [Description("R/W,系统变量Bbool24")]
        public bool Bbool204;
        [Description("R/W,系统变量Bbool25")]
        public bool Bbool205;
        [Description("R/W,系统变量Bbool26")]
        public bool Bbool206;
        [Description("R/W,系统变量Bbool27")]
        public bool Bbool207;
        [Description("R/W,系统变量Bbool28")]
        public bool Bbool208;
        [Description("R/W,系统变量Bbool29")]
        public bool Bbool209;
        [Description("R/W,系统变量Bbool210")]
        public bool Bbool210;
        [Description("R/W,系统变量Bbool211")]
        public bool Bbool211;
        [Description("R/W,系统变量Bbool212")]
        public bool Bbool212;
        [Description("R/W,系统变量Bbool213")]
        public bool Bbool213;
        [Description("R/W,系统变量Bbool214")]
        public bool Bbool214;
        [Description("R/W,系统变量Bbool215")]
        public bool Bbool215;
        [Description("R/W,系统变量Bbool216")]
        public bool Bbool216;
        [Description("R/W,系统变量Bbool217")]
        public bool Bbool217;
        [Description("R/W,系统变量Bbool218")]
        public bool Bbool218;
        [Description("R/W,系统变量Bbool219")]
        public bool Bbool219;
        [Description("R/W,系统变量Bbool220")]
        public bool Bbool220;
        [Description("R/W,系统变量Bbool221")]
        public bool Bbool221;
        [Description("R/W,系统变量Bbool222")]
        public bool Bbool222;
        [Description("R/W,系统变量Bbool223")]
        public bool Bbool223;
        [Description("R/W,系统变量Bbool224")]
        public bool Bbool224;
        [Description("R/W,系统变量Bbool225")]
        public bool Bbool225;
        [Description("R/W,系统变量Bbool226")]
        public bool Bbool226;
        [Description("R/W,系统变量Bbool227")]
        public bool Bbool227;
        [Description("R/W,系统变量Bbool228")]
        public bool Bbool228;
        [Description("R/W,系统变量Bbool229")]
        public bool Bbool229;
        [Description("R/W,系统变量Bbool230")]
        public bool Bbool230;
        [Description("R/W,系统变量Bbool231")]
        public bool Bbool231;
        [Description("R/W,系统变量Bbool232")]
        public bool Bbool232;
        [Description("R/W,系统变量Bbool233")]
        public bool Bbool233;
        [Description("R/W,系统变量Bbool234")]
        public bool Bbool234;
        [Description("R/W,系统变量Bbool235")]
        public bool Bbool235;
        [Description("R/W,系统变量Bbool236")]
        public bool Bbool236;
        [Description("R/W,系统变量Bbool237")]
        public bool Bbool237;
        [Description("R/W,系统变量Bbool238")]
        public bool Bbool238;
        [Description("R/W,系统变量Bbool239")]
        public bool Bbool239;
        [Description("R/W,系统变量Bbool240")]
        public bool Bbool240;
        [Description("R/W,系统变量Bbool241")]
        public bool Bbool241;
        [Description("R/W,系统变量Bbool242")]
        public bool Bbool242;
        [Description("R/W,系统变量Bbool243")]
        public bool Bbool243;
        [Description("R/W,系统变量Bbool244")]
        public bool Bbool244;
        [Description("R/W,系统变量Bbool245")]
        public bool Bbool245;
        [Description("R/W,系统变量Bbool246")]
        public bool Bbool246;
        [Description("R/W,系统变量Bbool247")]
        public bool Bbool247;
        [Description("R/W,系统变量Bbool248")]
        public bool Bbool248;
        [Description("R/W,系统变量Bbool249")]
        public bool Bbool249;
        [Description("R/W,系统变量Bbool250")]
        public bool Bbool250;
        [Description("R/W,系统变量Bbool251")]
        public bool Bbool251;
        [Description("R/W,系统变量Bbool252")]
        public bool Bbool252;
        [Description("R/W,系统变量Bbool253")]
        public bool Bbool253;
        [Description("R/W,系统变量Bbool254")]
        public bool Bbool254;
        [Description("R/W,系统变量Bbool255")]
        public bool Bbool255;
        [Description("R/W,系统变量Bbool256")]
        public bool Bbool256;
        [Description("R/W,系统变量Bbool257")]
        public bool Bbool257;
        [Description("R/W,系统变量Bbool258")]
        public bool Bbool258;
        [Description("R/W,系统变量Bbool259")]
        public bool Bbool259;
        [Description("R/W,系统变量Bbool260")]
        public bool Bbool260;
        [Description("R/W,系统变量Bbool261")]
        public bool Bbool261;
        [Description("R/W,系统变量Bbool262")]
        public bool Bbool262;
        [Description("R/W,系统变量Bbool263")]
        public bool Bbool263;
        [Description("R/W,系统变量Bbool264")]
        public bool Bbool264;
        [Description("R/W,系统变量Bbool265")]
        public bool Bbool265;
        [Description("R/W,系统变量Bbool266")]
        public bool Bbool266;
        [Description("R/W,系统变量Bbool267")]
        public bool Bbool267;
        [Description("R/W,系统变量Bbool268")]
        public bool Bbool268;
        [Description("R/W,系统变量Bbool269")]
        public bool Bbool269;
        [Description("R/W,系统变量Bbool270")]
        public bool Bbool270;
        [Description("R/W,系统变量Bbool271")]
        public bool Bbool271;
        [Description("R/W,系统变量Bbool272")]
        public bool Bbool272;
        [Description("R/W,系统变量Bbool273")]
        public bool Bbool273;
        [Description("R/W,系统变量Bbool274")]
        public bool Bbool274;
        [Description("R/W,系统变量Bbool275")]
        public bool Bbool275;
        [Description("R/W,系统变量Bbool276")]
        public bool Bbool276;
        [Description("R/W,系统变量Bbool277")]
        public bool Bbool277;
        [Description("R/W,系统变量Bbool278")]
        public bool Bbool278;
        [Description("R/W,系统变量Bbool279")]
        public bool Bbool279;
        [Description("R/W,系统变量Bbool280")]
        public bool Bbool280;
        [Description("R/W,系统变量Bbool281")]
        public bool Bbool281;
        [Description("R/W,系统变量Bbool282")]
        public bool Bbool282;
        [Description("R/W,系统变量Bbool283")]
        public bool Bbool283;
        [Description("R/W,系统变量Bbool284")]
        public bool Bbool284;
        [Description("R/W,系统变量Bbool285")]
        public bool Bbool285;
        [Description("R/W,系统变量Bbool286")]
        public bool Bbool286;
        [Description("R/W,系统变量Bbool287")]
        public bool Bbool287;
        [Description("R/W,系统变量Bbool288")]
        public bool Bbool288;
        [Description("R/W,系统变量Bbool289")]
        public bool Bbool289;
        [Description("R/W,系统变量Bbool290")]
        public bool Bbool290;
        [Description("R/W,系统变量Bbool291")]
        public bool Bbool291;
        [Description("R/W,系统变量Bbool292")]
        public bool Bbool292;
        [Description("R/W,系统变量Bbool293")]
        public bool Bbool293;
        [Description("R/W,系统变量Bbool294")]
        public bool Bbool294;
        [Description("R/W,系统变量Bbool295")]
        public bool Bbool295;
        [Description("R/W,系统变量Bbool296")]
        public bool Bbool296;
        [Description("R/W,系统变量Bbool297")]
        public bool Bbool297;
        [Description("R/W,系统变量Bbool298")]
        public bool Bbool298;
        [Description("R/W,系统变量Bbool299")]
        public bool Bbool299;
        #endregion

        #region Int型变量
        [Description("R/W,系统变量Bint0")]
        public int Bint0;
        [Description("R/W,系统变量Bint1")]
        public int Bint1;
        [Description("R/W,系统变量Bint2")]
        public int Bint2;
        [Description("R/W,系统变量Bint3")]
        public int Bint3;
        [Description("R/W,系统变量Bint4")]
        public int Bint4;
        [Description("R/W,系统变量Bint5")]
        public int Bint5;
        [Description("R/W,系统变量Bint6")]
        public int Bint6;
        [Description("R/W,系统变量Bint7")]
        public int Bint7;
        [Description("R/W,系统变量Bint8")]
        public int Bint8;
        [Description("R/W,系统变量Bint9")]
        public int Bint9;
        [Description("R/W,系统变量Bint10")]
        public int Bint10;
        [Description("R/W,系统变量Bint11")]
        public int Bint11;
        [Description("R/W,系统变量Bint12")]
        public int Bint12;
        [Description("R/W,系统变量Bint13")]
        public int Bint13;
        [Description("R/W,系统变量Bint14")]
        public int Bint14;
        [Description("R/W,系统变量Bint15")]
        public int Bint15;
        [Description("R/W,系统变量Bint16")]
        public int Bint16;
        [Description("R/W,系统变量Bint17")]
        public int Bint17;
        [Description("R/W,系统变量Bint18")]
        public int Bint18;
        [Description("R/W,系统变量Bint19")]
        public int Bint19;
        [Description("R/W,系统变量Bint20")]
        public int Bint20;
        [Description("R/W,系统变量Bint21")]
        public int Bint21;
        [Description("R/W,系统变量Bint22")]
        public int Bint22;
        [Description("R/W,系统变量Bint23")]
        public int Bint23;
        [Description("R/W,系统变量Bint24")]
        public int Bint24;
        [Description("R/W,系统变量Bint25")]
        public int Bint25;
        [Description("R/W,系统变量Bint26")]
        public int Bint26;
        [Description("R/W,系统变量Bint27")]
        public int Bint27;
        [Description("R/W,系统变量Bint28")]
        public int Bint28;
        [Description("R/W,系统变量Bint29")]
        public int Bint29;
        [Description("R/W,系统变量Bint30")]
        public int Bint30;
        [Description("R/W,系统变量Bint31")]
        public int Bint31;
        [Description("R/W,系统变量Bint32")]
        public int Bint32;
        [Description("R/W,系统变量Bint33")]
        public int Bint33;
        [Description("R/W,系统变量Bint34")]
        public int Bint34;
        [Description("R/W,系统变量Bint35")]
        public int Bint35;
        [Description("R/W,系统变量Bint36")]
        public int Bint36;
        [Description("R/W,系统变量Bint37")]
        public int Bint37;
        [Description("R/W,系统变量Bint38")]
        public int Bint38;
        [Description("R/W,系统变量Bint39")]
        public int Bint39;
        [Description("R/W,系统变量Bint40")]
        public int Bint40;
        [Description("R/W,系统变量Bint41")]
        public int Bint41;
        [Description("R/W,系统变量Bint42")]
        public int Bint42;
        [Description("R/W,系统变量Bint43")]
        public int Bint43;
        [Description("R/W,系统变量Bint44")]
        public int Bint44;
        [Description("R/W,系统变量Bint45")]
        public int Bint45;
        [Description("R/W,系统变量Bint46")]
        public int Bint46;
        [Description("R/W,系统变量Bint47")]
        public int Bint47;
        [Description("R/W,系统变量Bint48")]
        public int Bint48;
        [Description("R/W,系统变量Bint49")]
        public int Bint49;
        [Description("R/W,系统变量Bint50")]
        public int Bint50;
        [Description("R/W,系统变量Bint51")]
        public int Bint51;
        [Description("R/W,系统变量Bint52")]
        public int Bint52;
        [Description("R/W,系统变量Bint53")]
        public int Bint53;
        [Description("R/W,系统变量Bint54")]
        public int Bint54;
        [Description("R/W,系统变量Bint55")]
        public int Bint55;
        [Description("R/W,系统变量Bint56")]
        public int Bint56;
        [Description("R/W,系统变量Bint57")]
        public int Bint57;
        [Description("R/W,系统变量Bint58")]
        public int Bint58;
        [Description("R/W,系统变量Bint59")]
        public int Bint59;
        [Description("R/W,系统变量Bint60")]
        public int Bint60;
        [Description("R/W,系统变量Bint61")]
        public int Bint61;
        [Description("R/W,系统变量Bint62")]
        public int Bint62;
        [Description("R/W,系统变量Bint63")]
        public int Bint63;
        [Description("R/W,系统变量Bint64")]
        public int Bint64;
        [Description("R/W,系统变量Bint65")]
        public int Bint65;
        [Description("R/W,系统变量Bint66")]
        public int Bint66;
        [Description("R/W,系统变量Bint67")]
        public int Bint67;
        [Description("R/W,系统变量Bint68")]
        public int Bint68;
        [Description("R/W,系统变量Bint69")]
        public int Bint69;
        [Description("R/W,系统变量Bint70")]
        public int Bint70;
        [Description("R/W,系统变量Bint71")]
        public int Bint71;
        [Description("R/W,系统变量Bint72")]
        public int Bint72;
        [Description("R/W,系统变量Bint73")]
        public int Bint73;
        [Description("R/W,系统变量Bint74")]
        public int Bint74;
        [Description("R/W,系统变量Bint75")]
        public int Bint75;
        [Description("R/W,系统变量Bint76")]
        public int Bint76;
        [Description("R/W,系统变量Bint77")]
        public int Bint77;
        [Description("R/W,系统变量Bint78")]
        public int Bint78;
        [Description("R/W,系统变量Bint79")]
        public int Bint79;
        [Description("R/W,系统变量Bint80")]
        public int Bint80;
        [Description("R/W,系统变量Bint81")]
        public int Bint81;
        [Description("R/W,系统变量Bint82")]
        public int Bint82;
        [Description("R/W,系统变量Bint83")]
        public int Bint83;
        [Description("R/W,系统变量Bint84")]
        public int Bint84;
        [Description("R/W,系统变量Bint85")]
        public int Bint85;
        [Description("R/W,系统变量Bint86")]
        public int Bint86;
        [Description("R/W,系统变量Bint87")]
        public int Bint87;
        [Description("R/W,系统变量Bint88")]
        public int Bint88;
        [Description("R/W,系统变量Bint89")]
        public int Bint89;
        [Description("R/W,系统变量Bint90")]
        public int Bint90;
        [Description("R/W,系统变量Bint91")]
        public int Bint91;
        [Description("R/W,系统变量Bint92")]
        public int Bint92;
        [Description("R/W,系统变量Bint93")]
        public int Bint93;
        [Description("R/W,系统变量Bint94")]
        public int Bint94;
        [Description("R/W,系统变量Bint95")]
        public int Bint95;
        [Description("R/W,系统变量Bint96")]
        public int Bint96;
        [Description("R/W,系统变量Bint97")]
        public int Bint97;
        [Description("R/W,系统变量Bint98")]
        public int Bint98;
        [Description("R/W,系统变量Bint99")]
        public int Bint99;
        [Description("R/W,系统变量Bint100")]
        public int Bint100;
        [Description("R/W,系统变量Bint101")]
        public int Bint101;
        [Description("R/W,系统变量Bint102")]
        public int Bint102;
        [Description("R/W,系统变量Bint103")]
        public int Bint103;
        [Description("R/W,系统变量Bint104")]
        public int Bint104;
        [Description("R/W,系统变量Bint105")]
        public int Bint105;
        [Description("R/W,系统变量Bint106")]
        public int Bint106;
        [Description("R/W,系统变量Bint107")]
        public int Bint107;
        [Description("R/W,系统变量Bint108")]
        public int Bint108;
        [Description("R/W,系统变量Bint109")]
        public int Bint109;
        [Description("R/W,系统变量Bint110")]
        public int Bint110;
        [Description("R/W,系统变量Bint111")]
        public int Bint111;
        [Description("R/W,系统变量Bint112")]
        public int Bint112;
        [Description("R/W,系统变量Bint113")]
        public int Bint113;
        [Description("R/W,系统变量Bint114")]
        public int Bint114;
        [Description("R/W,系统变量Bint115")]
        public int Bint115;
        [Description("R/W,系统变量Bint116")]
        public int Bint116;
        [Description("R/W,系统变量Bint117")]
        public int Bint117;
        [Description("R/W,系统变量Bint118")]
        public int Bint118;
        [Description("R/W,系统变量Bint119")]
        public int Bint119;
        [Description("R/W,系统变量Bint120")]
        public int Bint120;
        [Description("R/W,系统变量Bint121")]
        public int Bint121;
        [Description("R/W,系统变量Bint122")]
        public int Bint122;
        [Description("R/W,系统变量Bint123")]
        public int Bint123;
        [Description("R/W,系统变量Bint124")]
        public int Bint124;
        [Description("R/W,系统变量Bint125")]
        public int Bint125;
        [Description("R/W,系统变量Bint126")]
        public int Bint126;
        [Description("R/W,系统变量Bint127")]
        public int Bint127;
        [Description("R/W,系统变量Bint128")]
        public int Bint128;
        [Description("R/W,系统变量Bint129")]
        public int Bint129;
        [Description("R/W,系统变量Bint130")]
        public int Bint130;
        [Description("R/W,系统变量Bint131")]
        public int Bint131;
        [Description("R/W,系统变量Bint132")]
        public int Bint132;
        [Description("R/W,系统变量Bint133")]
        public int Bint133;
        [Description("R/W,系统变量Bint134")]
        public int Bint134;
        [Description("R/W,系统变量Bint135")]
        public int Bint135;
        [Description("R/W,系统变量Bint136")]
        public int Bint136;
        [Description("R/W,系统变量Bint137")]
        public int Bint137;
        [Description("R/W,系统变量Bint138")]
        public int Bint138;
        [Description("R/W,系统变量Bint139")]
        public int Bint139;
        [Description("R/W,系统变量Bint140")]
        public int Bint140;
        [Description("R/W,系统变量Bint141")]
        public int Bint141;
        [Description("R/W,系统变量Bint142")]
        public int Bint142;
        [Description("R/W,系统变量Bint143")]
        public int Bint143;
        [Description("R/W,系统变量Bint144")]
        public int Bint144;
        [Description("R/W,系统变量Bint145")]
        public int Bint145;
        [Description("R/W,系统变量Bint146")]
        public int Bint146;
        [Description("R/W,系统变量Bint147")]
        public int Bint147;
        [Description("R/W,系统变量Bint148")]
        public int Bint148;
        [Description("R/W,系统变量Bint149")]
        public int Bint149;
        [Description("R/W,系统变量Bint150")]
        public int Bint150;
        [Description("R/W,系统变量Bint151")]
        public int Bint151;
        [Description("R/W,系统变量Bint152")]
        public int Bint152;
        [Description("R/W,系统变量Bint153")]
        public int Bint153;
        [Description("R/W,系统变量Bint154")]
        public int Bint154;
        [Description("R/W,系统变量Bint155")]
        public int Bint155;
        [Description("R/W,系统变量Bint156")]
        public int Bint156;
        [Description("R/W,系统变量Bint157")]
        public int Bint157;
        [Description("R/W,系统变量Bint158")]
        public int Bint158;
        [Description("R/W,系统变量Bint159")]
        public int Bint159;
        [Description("R/W,系统变量Bint160")]
        public int Bint160;
        [Description("R/W,系统变量Bint161")]
        public int Bint161;
        [Description("R/W,系统变量Bint162")]
        public int Bint162;
        [Description("R/W,系统变量Bint163")]
        public int Bint163;
        [Description("R/W,系统变量Bint164")]
        public int Bint164;
        [Description("R/W,系统变量Bint165")]
        public int Bint165;
        [Description("R/W,系统变量Bint166")]
        public int Bint166;
        [Description("R/W,系统变量Bint167")]
        public int Bint167;
        [Description("R/W,系统变量Bint168")]
        public int Bint168;
        [Description("R/W,系统变量Bint169")]
        public int Bint169;
        [Description("R/W,系统变量Bint170")]
        public int Bint170;
        [Description("R/W,系统变量Bint171")]
        public int Bint171;
        [Description("R/W,系统变量Bint172")]
        public int Bint172;
        [Description("R/W,系统变量Bint173")]
        public int Bint173;
        [Description("R/W,系统变量Bint174")]
        public int Bint174;
        [Description("R/W,系统变量Bint175")]
        public int Bint175;
        [Description("R/W,系统变量Bint176")]
        public int Bint176;
        [Description("R/W,系统变量Bint177")]
        public int Bint177;
        [Description("R/W,系统变量Bint178")]
        public int Bint178;
        [Description("R/W,系统变量Bint179")]
        public int Bint179;
        [Description("R/W,系统变量Bint180")]
        public int Bint180;
        [Description("R/W,系统变量Bint181")]
        public int Bint181;
        [Description("R/W,系统变量Bint182")]
        public int Bint182;
        [Description("R/W,系统变量Bint183")]
        public int Bint183;
        [Description("R/W,系统变量Bint184")]
        public int Bint184;
        [Description("R/W,系统变量Bint185")]
        public int Bint185;
        [Description("R/W,系统变量Bint186")]
        public int Bint186;
        [Description("R/W,系统变量Bint187")]
        public int Bint187;
        [Description("R/W,系统变量Bint188")]
        public int Bint188;
        [Description("R/W,系统变量Bint189")]
        public int Bint189;
        [Description("R/W,系统变量Bint190")]
        public int Bint190;
        [Description("R/W,系统变量Bint191")]
        public int Bint191;
        [Description("R/W,系统变量Bint192")]
        public int Bint192;
        [Description("R/W,系统变量Bint193")]
        public int Bint193;
        [Description("R/W,系统变量Bint194")]
        public int Bint194;
        [Description("R/W,系统变量Bint195")]
        public int Bint195;
        [Description("R/W,系统变量Bint196")]
        public int Bint196;
        [Description("R/W,系统变量Bint197")]
        public int Bint197;
        [Description("R/W,系统变量Bint198")]
        public int Bint198;
        [Description("R/W,系统变量Bint199")]
        public int Bint199;
        [Description("R/W,系统变量Bint20")]
        public int Bint200;
        [Description("R/W,系统变量Bint201")]
        public int Bint201;
        [Description("R/W,系统变量Bint202")]
        public int Bint202;
        [Description("R/W,系统变量Bint203")]
        public int Bint203;
        [Description("R/W,系统变量Bint204")]
        public int Bint204;
        [Description("R/W,系统变量Bint205")]
        public int Bint205;
        [Description("R/W,系统变量Bint206")]
        public int Bint206;
        [Description("R/W,系统变量Bint207")]
        public int Bint207;
        [Description("R/W,系统变量Bint208")]
        public int Bint208;
        [Description("R/W,系统变量Bint209")]
        public int Bint209;
        [Description("R/W,系统变量Bint210")]
        public int Bint210;
        [Description("R/W,系统变量Bint211")]
        public int Bint211;
        [Description("R/W,系统变量Bint212")]
        public int Bint212;
        [Description("R/W,系统变量Bint213")]
        public int Bint213;
        [Description("R/W,系统变量Bint214")]
        public int Bint214;
        [Description("R/W,系统变量Bint215")]
        public int Bint215;
        [Description("R/W,系统变量Bint216")]
        public int Bint216;
        [Description("R/W,系统变量Bint217")]
        public int Bint217;
        [Description("R/W,系统变量Bint218")]
        public int Bint218;
        [Description("R/W,系统变量Bint219")]
        public int Bint219;
        [Description("R/W,系统变量Bint220")]
        public int Bint220;
        [Description("R/W,系统变量Bint221")]
        public int Bint221;
        [Description("R/W,系统变量Bint222")]
        public int Bint222;
        [Description("R/W,系统变量Bint223")]
        public int Bint223;
        [Description("R/W,系统变量Bint224")]
        public int Bint224;
        [Description("R/W,系统变量Bint225")]
        public int Bint225;
        [Description("R/W,系统变量Bint226")]
        public int Bint226;
        [Description("R/W,系统变量Bint227")]
        public int Bint227;
        [Description("R/W,系统变量Bint228")]
        public int Bint228;
        [Description("R/W,系统变量Bint229")]
        public int Bint229;
        [Description("R/W,系统变量Bint230")]
        public int Bint230;
        [Description("R/W,系统变量Bint231")]
        public int Bint231;
        [Description("R/W,系统变量Bint232")]
        public int Bint232;
        [Description("R/W,系统变量Bint233")]
        public int Bint233;
        [Description("R/W,系统变量Bint234")]
        public int Bint234;
        [Description("R/W,系统变量Bint235")]
        public int Bint235;
        [Description("R/W,系统变量Bint236")]
        public int Bint236;
        [Description("R/W,系统变量Bint237")]
        public int Bint237;
        [Description("R/W,系统变量Bint238")]
        public int Bint238;
        [Description("R/W,系统变量Bint239")]
        public int Bint239;
        [Description("R/W,系统变量Bint240")]
        public int Bint240;
        [Description("R/W,系统变量Bint241")]
        public int Bint241;
        [Description("R/W,系统变量Bint242")]
        public int Bint242;
        [Description("R/W,系统变量Bint243")]
        public int Bint243;
        [Description("R/W,系统变量Bint244")]
        public int Bint244;
        [Description("R/W,系统变量Bint245")]
        public int Bint245;
        [Description("R/W,系统变量Bint246")]
        public int Bint246;
        [Description("R/W,系统变量Bint247")]
        public int Bint247;
        [Description("R/W,系统变量Bint248")]
        public int Bint248;
        [Description("R/W,系统变量Bint249")]
        public int Bint249;
        [Description("R/W,系统变量Bint250")]
        public int Bint250;
        [Description("R/W,系统变量Bint251")]
        public int Bint251;
        [Description("R/W,系统变量Bint252")]
        public int Bint252;
        [Description("R/W,系统变量Bint253")]
        public int Bint253;
        [Description("R/W,系统变量Bint254")]
        public int Bint254;
        [Description("R/W,系统变量Bint255")]
        public int Bint255;
        [Description("R/W,系统变量Bint256")]
        public int Bint256;
        [Description("R/W,系统变量Bint257")]
        public int Bint257;
        [Description("R/W,系统变量Bint258")]
        public int Bint258;
        [Description("R/W,系统变量Bint259")]
        public int Bint259;
        [Description("R/W,系统变量Bint260")]
        public int Bint260;
        [Description("R/W,系统变量Bint261")]
        public int Bint261;
        [Description("R/W,系统变量Bint262")]
        public int Bint262;
        [Description("R/W,系统变量Bint263")]
        public int Bint263;
        [Description("R/W,系统变量Bint264")]
        public int Bint264;
        [Description("R/W,系统变量Bint265")]
        public int Bint265;
        [Description("R/W,系统变量Bint266")]
        public int Bint266;
        [Description("R/W,系统变量Bint267")]
        public int Bint267;
        [Description("R/W,系统变量Bint268")]
        public int Bint268;
        [Description("R/W,系统变量Bint269")]
        public int Bint269;
        [Description("R/W,系统变量Bint270")]
        public int Bint270;
        [Description("R/W,系统变量Bint271")]
        public int Bint271;
        [Description("R/W,系统变量Bint272")]
        public int Bint272;
        [Description("R/W,系统变量Bint273")]
        public int Bint273;
        [Description("R/W,系统变量Bint274")]
        public int Bint274;
        [Description("R/W,系统变量Bint275")]
        public int Bint275;
        [Description("R/W,系统变量Bint276")]
        public int Bint276;
        [Description("R/W,系统变量Bint277")]
        public int Bint277;
        [Description("R/W,系统变量Bint278")]
        public int Bint278;
        [Description("R/W,系统变量Bint279")]
        public int Bint279;
        [Description("R/W,系统变量Bint280")]
        public int Bint280;
        [Description("R/W,系统变量Bint281")]
        public int Bint281;
        [Description("R/W,系统变量Bint282")]
        public int Bint282;
        [Description("R/W,系统变量Bint283")]
        public int Bint283;
        [Description("R/W,系统变量Bint284")]
        public int Bint284;
        [Description("R/W,系统变量Bint285")]
        public int Bint285;
        [Description("R/W,系统变量Bint286")]
        public int Bint286;
        [Description("R/W,系统变量Bint287")]
        public int Bint287;
        [Description("R/W,系统变量Bint288")]
        public int Bint288;
        [Description("R/W,系统变量Bint289")]
        public int Bint289;
        [Description("R/W,系统变量Bint290")]
        public int Bint290;
        [Description("R/W,系统变量Bint291")]
        public int Bint291;
        [Description("R/W,系统变量Bint292")]
        public int Bint292;
        [Description("R/W,系统变量Bint293")]
        public int Bint293;
        [Description("R/W,系统变量Bint294")]
        public int Bint294;
        [Description("R/W,系统变量Bint295")]
        public int Bint295;
        [Description("R/W,系统变量Bint296")]
        public int Bint296;
        [Description("R/W,系统变量Bint297")]
        public int Bint297;
        [Description("R/W,系统变量Bint298")]
        public int Bint298;
        [Description("R/W,系统变量Bint299")]
        public int Bint299;
        #endregion

        #region double型变量
        [Description("R/W,系统变量Bdouble0")]
        public double Bdouble0;
        [Description("R/W,系统变量Bdouble1")]
        public double Bdouble1;
        [Description("R/W,系统变量Bdouble2")]
        public double Bdouble2;
        [Description("R/W,系统变量Bdouble3")]
        public double Bdouble3;
        [Description("R/W,系统变量Bdouble4")]
        public double Bdouble4;
        [Description("R/W,系统变量Bdouble5")]
        public double Bdouble5;
        [Description("R/W,系统变量Bdouble6")]
        public double Bdouble6;
        [Description("R/W,系统变量Bdouble7")]
        public double Bdouble7;
        [Description("R/W,系统变量Bdouble8")]
        public double Bdouble8;
        [Description("R/W,系统变量Bdouble9")]
        public double Bdouble9;
        [Description("R/W,系统变量Bdouble10")]
        public double Bdouble10;
        [Description("R/W,系统变量Bdouble11")]
        public double Bdouble11;
        [Description("R/W,系统变量Bdouble12")]
        public double Bdouble12;
        [Description("R/W,系统变量Bdouble13")]
        public double Bdouble13;
        [Description("R/W,系统变量Bdouble14")]
        public double Bdouble14;
        [Description("R/W,系统变量Bdouble15")]
        public double Bdouble15;
        [Description("R/W,系统变量Bdouble16")]
        public double Bdouble16;
        [Description("R/W,系统变量Bdouble17")]
        public double Bdouble17;
        [Description("R/W,系统变量Bdouble18")]
        public double Bdouble18;
        [Description("R/W,系统变量Bdouble19")]
        public double Bdouble19;
        [Description("R/W,系统变量Bdouble20")]
        public double Bdouble20;
        [Description("R/W,系统变量Bdouble21")]
        public double Bdouble21;
        [Description("R/W,系统变量Bdouble22")]
        public double Bdouble22;
        [Description("R/W,系统变量Bdouble23")]
        public double Bdouble23;
        [Description("R/W,系统变量Bdouble24")]
        public double Bdouble24;
        [Description("R/W,系统变量Bdouble25")]
        public double Bdouble25;
        [Description("R/W,系统变量Bdouble26")]
        public double Bdouble26;
        [Description("R/W,系统变量Bdouble27")]
        public double Bdouble27;
        [Description("R/W,系统变量Bdouble28")]
        public double Bdouble28;
        [Description("R/W,系统变量Bdouble29")]
        public double Bdouble29;
        [Description("R/W,系统变量Bdouble30")]
        public double Bdouble30;
        [Description("R/W,系统变量Bdouble31")]
        public double Bdouble31;
        [Description("R/W,系统变量Bdouble32")]
        public double Bdouble32;
        [Description("R/W,系统变量Bdouble33")]
        public double Bdouble33;
        [Description("R/W,系统变量Bdouble34")]
        public double Bdouble34;
        [Description("R/W,系统变量Bdouble35")]
        public double Bdouble35;
        [Description("R/W,系统变量Bdouble36")]
        public double Bdouble36;
        [Description("R/W,系统变量Bdouble37")]
        public double Bdouble37;
        [Description("R/W,系统变量Bdouble38")]
        public double Bdouble38;
        [Description("R/W,系统变量Bdouble39")]
        public double Bdouble39;
        [Description("R/W,系统变量Bdouble40")]
        public double Bdouble40;
        [Description("R/W,系统变量Bdouble41")]
        public double Bdouble41;
        [Description("R/W,系统变量Bdouble42")]
        public double Bdouble42;
        [Description("R/W,系统变量Bdouble43")]
        public double Bdouble43;
        [Description("R/W,系统变量Bdouble44")]
        public double Bdouble44;
        [Description("R/W,系统变量Bdouble45")]
        public double Bdouble45;
        [Description("R/W,系统变量Bdouble46")]
        public double Bdouble46;
        [Description("R/W,系统变量Bdouble47")]
        public double Bdouble47;
        [Description("R/W,系统变量Bdouble48")]
        public double Bdouble48;
        [Description("R/W,系统变量Bdouble49")]
        public double Bdouble49;
        [Description("R/W,系统变量Bdouble50")]
        public double Bdouble50;
        [Description("R/W,系统变量Bdouble51")]
        public double Bdouble51;
        [Description("R/W,系统变量Bdouble52")]
        public double Bdouble52;
        [Description("R/W,系统变量Bdouble53")]
        public double Bdouble53;
        [Description("R/W,系统变量Bdouble54")]
        public double Bdouble54;
        [Description("R/W,系统变量Bdouble55")]
        public double Bdouble55;
        [Description("R/W,系统变量Bdouble56")]
        public double Bdouble56;
        [Description("R/W,系统变量Bdouble57")]
        public double Bdouble57;
        [Description("R/W,系统变量Bdouble58")]
        public double Bdouble58;
        [Description("R/W,系统变量Bdouble59")]
        public double Bdouble59;
        [Description("R/W,系统变量Bdouble60")]
        public double Bdouble60;
        [Description("R/W,系统变量Bdouble61")]
        public double Bdouble61;
        [Description("R/W,系统变量Bdouble62")]
        public double Bdouble62;
        [Description("R/W,系统变量Bdouble63")]
        public double Bdouble63;
        [Description("R/W,系统变量Bdouble64")]
        public double Bdouble64;
        [Description("R/W,系统变量Bdouble65")]
        public double Bdouble65;
        [Description("R/W,系统变量Bdouble66")]
        public double Bdouble66;
        [Description("R/W,系统变量Bdouble67")]
        public double Bdouble67;
        [Description("R/W,系统变量Bdouble68")]
        public double Bdouble68;
        [Description("R/W,系统变量Bdouble69")]
        public double Bdouble69;
        [Description("R/W,系统变量Bdouble70")]
        public double Bdouble70;
        [Description("R/W,系统变量Bdouble71")]
        public double Bdouble71;
        [Description("R/W,系统变量Bdouble72")]
        public double Bdouble72;
        [Description("R/W,系统变量Bdouble73")]
        public double Bdouble73;
        [Description("R/W,系统变量Bdouble74")]
        public double Bdouble74;
        [Description("R/W,系统变量Bdouble75")]
        public double Bdouble75;
        [Description("R/W,系统变量Bdouble76")]
        public double Bdouble76;
        [Description("R/W,系统变量Bdouble77")]
        public double Bdouble77;
        [Description("R/W,系统变量Bdouble78")]
        public double Bdouble78;
        [Description("R/W,系统变量Bdouble79")]
        public double Bdouble79;
        [Description("R/W,系统变量Bdouble80")]
        public double Bdouble80;
        [Description("R/W,系统变量Bdouble81")]
        public double Bdouble81;
        [Description("R/W,系统变量Bdouble82")]
        public double Bdouble82;
        [Description("R/W,系统变量Bdouble83")]
        public double Bdouble83;
        [Description("R/W,系统变量Bdouble84")]
        public double Bdouble84;
        [Description("R/W,系统变量Bdouble85")]
        public double Bdouble85;
        [Description("R/W,系统变量Bdouble86")]
        public double Bdouble86;
        [Description("R/W,系统变量Bdouble87")]
        public double Bdouble87;
        [Description("R/W,系统变量Bdouble88")]
        public double Bdouble88;
        [Description("R/W,系统变量Bdouble89")]
        public double Bdouble89;
        [Description("R/W,系统变量Bdouble90")]
        public double Bdouble90;
        [Description("R/W,系统变量Bdouble91")]
        public double Bdouble91;
        [Description("R/W,系统变量Bdouble92")]
        public double Bdouble92;
        [Description("R/W,系统变量Bdouble93")]
        public double Bdouble93;
        [Description("R/W,系统变量Bdouble94")]
        public double Bdouble94;
        [Description("R/W,系统变量Bdouble95")]
        public double Bdouble95;
        [Description("R/W,系统变量Bdouble96")]
        public double Bdouble96;
        [Description("R/W,系统变量Bdouble97")]
        public double Bdouble97;
        [Description("R/W,系统变量Bdouble98")]
        public double Bdouble98;
        [Description("R/W,系统变量Bdouble99")]
        public double Bdouble99;
        [Description("R/W,系统变量Bdouble100")]
        public double Bdouble100;
        [Description("R/W,系统变量Bdouble101")]
        public double Bdouble101;
        [Description("R/W,系统变量Bdouble102")]
        public double Bdouble102;
        [Description("R/W,系统变量Bdouble103")]
        public double Bdouble103;
        [Description("R/W,系统变量Bdouble104")]
        public double Bdouble104;
        [Description("R/W,系统变量Bdouble105")]
        public double Bdouble105;
        [Description("R/W,系统变量Bdouble106")]
        public double Bdouble106;
        [Description("R/W,系统变量Bdouble107")]
        public double Bdouble107;
        [Description("R/W,系统变量Bdouble108")]
        public double Bdouble108;
        [Description("R/W,系统变量Bdouble109")]
        public double Bdouble109;
        [Description("R/W,系统变量Bdouble110")]
        public double Bdouble110;
        [Description("R/W,系统变量Bdouble111")]
        public double Bdouble111;
        [Description("R/W,系统变量Bdouble112")]
        public double Bdouble112;
        [Description("R/W,系统变量Bdouble113")]
        public double Bdouble113;
        [Description("R/W,系统变量Bdouble114")]
        public double Bdouble114;
        [Description("R/W,系统变量Bdouble115")]
        public double Bdouble115;
        [Description("R/W,系统变量Bdouble116")]
        public double Bdouble116;
        [Description("R/W,系统变量Bdouble117")]
        public double Bdouble117;
        [Description("R/W,系统变量Bdouble118")]
        public double Bdouble118;
        [Description("R/W,系统变量Bdouble119")]
        public double Bdouble119;
        [Description("R/W,系统变量Bdouble120")]
        public double Bdouble120;
        [Description("R/W,系统变量Bdouble121")]
        public double Bdouble121;
        [Description("R/W,系统变量Bdouble122")]
        public double Bdouble122;
        [Description("R/W,系统变量Bdouble123")]
        public double Bdouble123;
        [Description("R/W,系统变量Bdouble124")]
        public double Bdouble124;
        [Description("R/W,系统变量Bdouble125")]
        public double Bdouble125;
        [Description("R/W,系统变量Bdouble126")]
        public double Bdouble126;
        [Description("R/W,系统变量Bdouble127")]
        public double Bdouble127;
        [Description("R/W,系统变量Bdouble128")]
        public double Bdouble128;
        [Description("R/W,系统变量Bdouble129")]
        public double Bdouble129;
        [Description("R/W,系统变量Bdouble130")]
        public double Bdouble130;
        [Description("R/W,系统变量Bdouble131")]
        public double Bdouble131;
        [Description("R/W,系统变量Bdouble132")]
        public double Bdouble132;
        [Description("R/W,系统变量Bdouble133")]
        public double Bdouble133;
        [Description("R/W,系统变量Bdouble134")]
        public double Bdouble134;
        [Description("R/W,系统变量Bdouble135")]
        public double Bdouble135;
        [Description("R/W,系统变量Bdouble136")]
        public double Bdouble136;
        [Description("R/W,系统变量Bdouble137")]
        public double Bdouble137;
        [Description("R/W,系统变量Bdouble138")]
        public double Bdouble138;
        [Description("R/W,系统变量Bdouble139")]
        public double Bdouble139;
        [Description("R/W,系统变量Bdouble140")]
        public double Bdouble140;
        [Description("R/W,系统变量Bdouble141")]
        public double Bdouble141;
        [Description("R/W,系统变量Bdouble142")]
        public double Bdouble142;
        [Description("R/W,系统变量Bdouble143")]
        public double Bdouble143;
        [Description("R/W,系统变量Bdouble144")]
        public double Bdouble144;
        [Description("R/W,系统变量Bdouble145")]
        public double Bdouble145;
        [Description("R/W,系统变量Bdouble146")]
        public double Bdouble146;
        [Description("R/W,系统变量Bdouble147")]
        public double Bdouble147;
        [Description("R/W,系统变量Bdouble148")]
        public double Bdouble148;
        [Description("R/W,系统变量Bdouble149")]
        public double Bdouble149;
        [Description("R/W,系统变量Bdouble150")]
        public double Bdouble150;
        [Description("R/W,系统变量Bdouble151")]
        public double Bdouble151;
        [Description("R/W,系统变量Bdouble152")]
        public double Bdouble152;
        [Description("R/W,系统变量Bdouble153")]
        public double Bdouble153;
        [Description("R/W,系统变量Bdouble154")]
        public double Bdouble154;
        [Description("R/W,系统变量Bdouble155")]
        public double Bdouble155;
        [Description("R/W,系统变量Bdouble156")]
        public double Bdouble156;
        [Description("R/W,系统变量Bdouble157")]
        public double Bdouble157;
        [Description("R/W,系统变量Bdouble158")]
        public double Bdouble158;
        [Description("R/W,系统变量Bdouble159")]
        public double Bdouble159;
        [Description("R/W,系统变量Bdouble160")]
        public double Bdouble160;
        [Description("R/W,系统变量Bdouble161")]
        public double Bdouble161;
        [Description("R/W,系统变量Bdouble162")]
        public double Bdouble162;
        [Description("R/W,系统变量Bdouble163")]
        public double Bdouble163;
        [Description("R/W,系统变量Bdouble164")]
        public double Bdouble164;
        [Description("R/W,系统变量Bdouble165")]
        public double Bdouble165;
        [Description("R/W,系统变量Bdouble166")]
        public double Bdouble166;
        [Description("R/W,系统变量Bdouble167")]
        public double Bdouble167;
        [Description("R/W,系统变量Bdouble168")]
        public double Bdouble168;
        [Description("R/W,系统变量Bdouble169")]
        public double Bdouble169;
        [Description("R/W,系统变量Bdouble170")]
        public double Bdouble170;
        [Description("R/W,系统变量Bdouble171")]
        public double Bdouble171;
        [Description("R/W,系统变量Bdouble172")]
        public double Bdouble172;
        [Description("R/W,系统变量Bdouble173")]
        public double Bdouble173;
        [Description("R/W,系统变量Bdouble174")]
        public double Bdouble174;
        [Description("R/W,系统变量Bdouble175")]
        public double Bdouble175;
        [Description("R/W,系统变量Bdouble176")]
        public double Bdouble176;
        [Description("R/W,系统变量Bdouble177")]
        public double Bdouble177;
        [Description("R/W,系统变量Bdouble178")]
        public double Bdouble178;
        [Description("R/W,系统变量Bdouble179")]
        public double Bdouble179;
        [Description("R/W,系统变量Bdouble180")]
        public double Bdouble180;
        [Description("R/W,系统变量Bdouble181")]
        public double Bdouble181;
        [Description("R/W,系统变量Bdouble182")]
        public double Bdouble182;
        [Description("R/W,系统变量Bdouble183")]
        public double Bdouble183;
        [Description("R/W,系统变量Bdouble184")]
        public double Bdouble184;
        [Description("R/W,系统变量Bdouble185")]
        public double Bdouble185;
        [Description("R/W,系统变量Bdouble186")]
        public double Bdouble186;
        [Description("R/W,系统变量Bdouble187")]
        public double Bdouble187;
        [Description("R/W,系统变量Bdouble188")]
        public double Bdouble188;
        [Description("R/W,系统变量Bdouble189")]
        public double Bdouble189;
        [Description("R/W,系统变量Bdouble190")]
        public double Bdouble190;
        [Description("R/W,系统变量Bdouble191")]
        public double Bdouble191;
        [Description("R/W,系统变量Bdouble192")]
        public double Bdouble192;
        [Description("R/W,系统变量Bdouble193")]
        public double Bdouble193;
        [Description("R/W,系统变量Bdouble194")]
        public double Bdouble194;
        [Description("R/W,系统变量Bdouble195")]
        public double Bdouble195;
        [Description("R/W,系统变量Bdouble196")]
        public double Bdouble196;
        [Description("R/W,系统变量Bdouble197")]
        public double Bdouble197;
        [Description("R/W,系统变量Bdouble198")]
        public double Bdouble198;
        [Description("R/W,系统变量Bdouble199")]
        public double Bdouble199;
        [Description("R/W,系统变量Bdouble200")]
        public double Bdouble200;
        [Description("R/W,系统变量Bdouble201")]
        public double Bdouble201;
        [Description("R/W,系统变量Bdouble202")]
        public double Bdouble202;
        [Description("R/W,系统变量Bdouble203")]
        public double Bdouble203;
        [Description("R/W,系统变量Bdouble204")]
        public double Bdouble204;
        [Description("R/W,系统变量Bdouble205")]
        public double Bdouble205;
        [Description("R/W,系统变量Bdouble206")]
        public double Bdouble206;
        [Description("R/W,系统变量Bdouble207")]
        public double Bdouble207;
        [Description("R/W,系统变量Bdouble208")]
        public double Bdouble208;
        [Description("R/W,系统变量Bdouble209")]
        public double Bdouble209;
        [Description("R/W,系统变量Bdouble210")]
        public double Bdouble210;
        [Description("R/W,系统变量Bdouble211")]
        public double Bdouble211;
        [Description("R/W,系统变量Bdouble212")]
        public double Bdouble212;
        [Description("R/W,系统变量Bdouble213")]
        public double Bdouble213;
        [Description("R/W,系统变量Bdouble214")]
        public double Bdouble214;
        [Description("R/W,系统变量Bdouble215")]
        public double Bdouble215;
        [Description("R/W,系统变量Bdouble216")]
        public double Bdouble216;
        [Description("R/W,系统变量Bdouble217")]
        public double Bdouble217;
        [Description("R/W,系统变量Bdouble218")]
        public double Bdouble218;
        [Description("R/W,系统变量Bdouble219")]
        public double Bdouble219;
        [Description("R/W,系统变量Bdouble220")]
        public double Bdouble220;
        [Description("R/W,系统变量Bdouble221")]
        public double Bdouble221;
        [Description("R/W,系统变量Bdouble222")]
        public double Bdouble222;
        [Description("R/W,系统变量Bdouble223")]
        public double Bdouble223;
        [Description("R/W,系统变量Bdouble224")]
        public double Bdouble224;
        [Description("R/W,系统变量Bdouble225")]
        public double Bdouble225;
        [Description("R/W,系统变量Bdouble226")]
        public double Bdouble226;
        [Description("R/W,系统变量Bdouble227")]
        public double Bdouble227;
        [Description("R/W,系统变量Bdouble228")]
        public double Bdouble228;
        [Description("R/W,系统变量Bdouble229")]
        public double Bdouble229;
        [Description("R/W,系统变量Bdouble230")]
        public double Bdouble230;
        [Description("R/W,系统变量Bdouble231")]
        public double Bdouble231;
        [Description("R/W,系统变量Bdouble232")]
        public double Bdouble232;
        [Description("R/W,系统变量Bdouble233")]
        public double Bdouble233;
        [Description("R/W,系统变量Bdouble234")]
        public double Bdouble234;
        [Description("R/W,系统变量Bdouble235")]
        public double Bdouble235;
        [Description("R/W,系统变量Bdouble236")]
        public double Bdouble236;
        [Description("R/W,系统变量Bdouble237")]
        public double Bdouble237;
        [Description("R/W,系统变量Bdouble238")]
        public double Bdouble238;
        [Description("R/W,系统变量Bdouble239")]
        public double Bdouble239;
        [Description("R/W,系统变量Bdouble240")]
        public double Bdouble240;
        [Description("R/W,系统变量Bdouble241")]
        public double Bdouble241;
        [Description("R/W,系统变量Bdouble242")]
        public double Bdouble242;
        [Description("R/W,系统变量Bdouble243")]
        public double Bdouble243;
        [Description("R/W,系统变量Bdouble244")]
        public double Bdouble244;
        [Description("R/W,系统变量Bdouble245")]
        public double Bdouble245;
        [Description("R/W,系统变量Bdouble246")]
        public double Bdouble246;
        [Description("R/W,系统变量Bdouble247")]
        public double Bdouble247;
        [Description("R/W,系统变量Bdouble248")]
        public double Bdouble248;
        [Description("R/W,系统变量Bdouble249")]
        public double Bdouble249;
        [Description("R/W,系统变量Bdouble250")]
        public double Bdouble250;
        [Description("R/W,系统变量Bdouble251")]
        public double Bdouble251;
        [Description("R/W,系统变量Bdouble252")]
        public double Bdouble252;
        [Description("R/W,系统变量Bdouble253")]
        public double Bdouble253;
        [Description("R/W,系统变量Bdouble254")]
        public double Bdouble254;
        [Description("R/W,系统变量Bdouble255")]
        public double Bdouble255;
        [Description("R/W,系统变量Bdouble256")]
        public double Bdouble256;
        [Description("R/W,系统变量Bdouble257")]
        public double Bdouble257;
        [Description("R/W,系统变量Bdouble258")]
        public double Bdouble258;
        [Description("R/W,系统变量Bdouble259")]
        public double Bdouble259;
        [Description("R/W,系统变量Bdouble260")]
        public double Bdouble260;
        [Description("R/W,系统变量Bdouble261")]
        public double Bdouble261;
        [Description("R/W,系统变量Bdouble262")]
        public double Bdouble262;
        [Description("R/W,系统变量Bdouble263")]
        public double Bdouble263;
        [Description("R/W,系统变量Bdouble264")]
        public double Bdouble264;
        [Description("R/W,系统变量Bdouble265")]
        public double Bdouble265;
        [Description("R/W,系统变量Bdouble266")]
        public double Bdouble266;
        [Description("R/W,系统变量Bdouble267")]
        public double Bdouble267;
        [Description("R/W,系统变量Bdouble268")]
        public double Bdouble268;
        [Description("R/W,系统变量Bdouble269")]
        public double Bdouble269;
        [Description("R/W,系统变量Bdouble270")]
        public double Bdouble270;
        [Description("R/W,系统变量Bdouble271")]
        public double Bdouble271;
        [Description("R/W,系统变量Bdouble272")]
        public double Bdouble272;
        [Description("R/W,系统变量Bdouble273")]
        public double Bdouble273;
        [Description("R/W,系统变量Bdouble274")]
        public double Bdouble274;
        [Description("R/W,系统变量Bdouble275")]
        public double Bdouble275;
        [Description("R/W,系统变量Bdouble276")]
        public double Bdouble276;
        [Description("R/W,系统变量Bdouble277")]
        public double Bdouble277;
        [Description("R/W,系统变量Bdouble278")]
        public double Bdouble278;
        [Description("R/W,系统变量Bdouble279")]
        public double Bdouble279;
        [Description("R/W,系统变量Bdouble280")]
        public double Bdouble280;
        [Description("R/W,系统变量Bdouble281")]
        public double Bdouble281;
        [Description("R/W,系统变量Bdouble282")]
        public double Bdouble282;
        [Description("R/W,系统变量Bdouble283")]
        public double Bdouble283;
        [Description("R/W,系统变量Bdouble284")]
        public double Bdouble284;
        [Description("R/W,系统变量Bdouble285")]
        public double Bdouble285;
        [Description("R/W,系统变量Bdouble286")]
        public double Bdouble286;
        [Description("R/W,系统变量Bdouble287")]
        public double Bdouble287;
        [Description("R/W,系统变量Bdouble288")]
        public double Bdouble288;
        [Description("R/W,系统变量Bdouble289")]
        public double Bdouble289;
        [Description("R/W,系统变量Bdouble290")]
        public double Bdouble290;
        [Description("R/W,系统变量Bdouble291")]
        public double Bdouble291;
        [Description("R/W,系统变量Bdouble292")]
        public double Bdouble292;
        [Description("R/W,系统变量Bdouble293")]
        public double Bdouble293;
        [Description("R/W,系统变量Bdouble294")]
        public double Bdouble294;
        [Description("R/W,系统变量Bdouble295")]
        public double Bdouble295;
        [Description("R/W,系统变量Bdouble296")]
        public double Bdouble296;
        [Description("R/W,系统变量Bdouble297")]
        public double Bdouble297;
        [Description("R/W,系统变量Bdouble298")]
        public double Bdouble298;
        [Description("R/W,系统变量Bdouble299")]
        public double Bdouble299;
        #endregion

        #region float型变量

        [Description("R/W,系统变量Bfloat0")]
        public float Bfloat0;
        [Description("R/W,系统变量Bfloat1")]
        public float Bfloat1;
        [Description("R/W,系统变量Bfloat2")]
        public float Bfloat2;
        [Description("R/W,系统变量Bfloat3")]
        public float Bfloat3;
        [Description("R/W,系统变量Bfloat4")]
        public float Bfloat4;
        [Description("R/W,系统变量Bfloat5")]
        public float Bfloat5;
        [Description("R/W,系统变量Bfloat6")]
        public float Bfloat6;
        [Description("R/W,系统变量Bfloat7")]
        public float Bfloat7;
        [Description("R/W,系统变量Bfloat8")]
        public float Bfloat8;
        [Description("R/W,系统变量Bfloat9")]
        public float Bfloat9;
        [Description("R/W,系统变量Bfloat10")]
        public float Bfloat10;
        [Description("R/W,系统变量Bfloat11")]
        public float Bfloat11;
        [Description("R/W,系统变量Bfloat12")]
        public float Bfloat12;
        [Description("R/W,系统变量Bfloat13")]
        public float Bfloat13;
        [Description("R/W,系统变量Bfloat14")]
        public float Bfloat14;
        [Description("R/W,系统变量Bfloat15")]
        public float Bfloat15;
        [Description("R/W,系统变量Bfloat16")]
        public float Bfloat16;
        [Description("R/W,系统变量Bfloat17")]
        public float Bfloat17;
        [Description("R/W,系统变量Bfloat18")]
        public float Bfloat18;
        [Description("R/W,系统变量Bfloat19")]
        public float Bfloat19;
        [Description("R/W,系统变量Bfloat20")]
        public float Bfloat20;
        [Description("R/W,系统变量Bfloat21")]
        public float Bfloat21;
        [Description("R/W,系统变量Bfloat22")]
        public float Bfloat22;
        [Description("R/W,系统变量Bfloat23")]
        public float Bfloat23;
        [Description("R/W,系统变量Bfloat24")]
        public float Bfloat24;
        [Description("R/W,系统变量Bfloat25")]
        public float Bfloat25;
        [Description("R/W,系统变量Bfloat26")]
        public float Bfloat26;
        [Description("R/W,系统变量Bfloat27")]
        public float Bfloat27;
        [Description("R/W,系统变量Bfloat28")]
        public float Bfloat28;
        [Description("R/W,系统变量Bfloat29")]
        public float Bfloat29;
        [Description("R/W,系统变量Bfloat30")]
        public float Bfloat30;
        [Description("R/W,系统变量Bfloat31")]
        public float Bfloat31;
        [Description("R/W,系统变量Bfloat32")]
        public float Bfloat32;
        [Description("R/W,系统变量Bfloat33")]
        public float Bfloat33;
        [Description("R/W,系统变量Bfloat34")]
        public float Bfloat34;
        [Description("R/W,系统变量Bfloat35")]
        public float Bfloat35;
        [Description("R/W,系统变量Bfloat36")]
        public float Bfloat36;
        [Description("R/W,系统变量Bfloat37")]
        public float Bfloat37;
        [Description("R/W,系统变量Bfloat38")]
        public float Bfloat38;
        [Description("R/W,系统变量Bfloat39")]
        public float Bfloat39;
        [Description("R/W,系统变量Bfloat40")]
        public float Bfloat40;
        [Description("R/W,系统变量Bfloat41")]
        public float Bfloat41;
        [Description("R/W,系统变量Bfloat42")]
        public float Bfloat42;
        [Description("R/W,系统变量Bfloat43")]
        public float Bfloat43;
        [Description("R/W,系统变量Bfloat44")]
        public float Bfloat44;
        [Description("R/W,系统变量Bfloat45")]
        public float Bfloat45;
        [Description("R/W,系统变量Bfloat46")]
        public float Bfloat46;
        [Description("R/W,系统变量Bfloat47")]
        public float Bfloat47;
        [Description("R/W,系统变量Bfloat48")]
        public float Bfloat48;
        [Description("R/W,系统变量Bfloat49")]
        public float Bfloat49;
        [Description("R/W,系统变量Bfloat50")]
        public float Bfloat50;
        [Description("R/W,系统变量Bfloat51")]
        public float Bfloat51;
        [Description("R/W,系统变量Bfloat52")]
        public float Bfloat52;
        [Description("R/W,系统变量Bfloat53")]
        public float Bfloat53;
        [Description("R/W,系统变量Bfloat54")]
        public float Bfloat54;
        [Description("R/W,系统变量Bfloat55")]
        public float Bfloat55;
        [Description("R/W,系统变量Bfloat56")]
        public float Bfloat56;
        [Description("R/W,系统变量Bfloat57")]
        public float Bfloat57;
        [Description("R/W,系统变量Bfloat58")]
        public float Bfloat58;
        [Description("R/W,系统变量Bfloat59")]
        public float Bfloat59;
        [Description("R/W,系统变量Bfloat60")]
        public float Bfloat60;
        [Description("R/W,系统变量Bfloat61")]
        public float Bfloat61;
        [Description("R/W,系统变量Bfloat62")]
        public float Bfloat62;
        [Description("R/W,系统变量Bfloat63")]
        public float Bfloat63;
        [Description("R/W,系统变量Bfloat64")]
        public float Bfloat64;
        [Description("R/W,系统变量Bfloat65")]
        public float Bfloat65;
        [Description("R/W,系统变量Bfloat66")]
        public float Bfloat66;
        [Description("R/W,系统变量Bfloat67")]
        public float Bfloat67;
        [Description("R/W,系统变量Bfloat68")]
        public float Bfloat68;
        [Description("R/W,系统变量Bfloat69")]
        public float Bfloat69;
        [Description("R/W,系统变量Bfloat70")]
        public float Bfloat70;
        [Description("R/W,系统变量Bfloat71")]
        public float Bfloat71;
        [Description("R/W,系统变量Bfloat72")]
        public float Bfloat72;
        [Description("R/W,系统变量Bfloat73")]
        public float Bfloat73;
        [Description("R/W,系统变量Bfloat74")]
        public float Bfloat74;
        [Description("R/W,系统变量Bfloat75")]
        public float Bfloat75;
        [Description("R/W,系统变量Bfloat76")]
        public float Bfloat76;
        [Description("R/W,系统变量Bfloat77")]
        public float Bfloat77;
        [Description("R/W,系统变量Bfloat78")]
        public float Bfloat78;
        [Description("R/W,系统变量Bfloat79")]
        public float Bfloat79;
        [Description("R/W,系统变量Bfloat80")]
        public float Bfloat80;
        [Description("R/W,系统变量Bfloat81")]
        public float Bfloat81;
        [Description("R/W,系统变量Bfloat82")]
        public float Bfloat82;
        [Description("R/W,系统变量Bfloat83")]
        public float Bfloat83;
        [Description("R/W,系统变量Bfloat84")]
        public float Bfloat84;
        [Description("R/W,系统变量Bfloat85")]
        public float Bfloat85;
        [Description("R/W,系统变量Bfloat86")]
        public float Bfloat86;
        [Description("R/W,系统变量Bfloat87")]
        public float Bfloat87;
        [Description("R/W,系统变量Bfloat88")]
        public float Bfloat88;
        [Description("R/W,系统变量Bfloat89")]
        public float Bfloat89;
        [Description("R/W,系统变量Bfloat90")]
        public float Bfloat90;
        [Description("R/W,系统变量Bfloat91")]
        public float Bfloat91;
        [Description("R/W,系统变量Bfloat92")]
        public float Bfloat92;
        [Description("R/W,系统变量Bfloat93")]
        public float Bfloat93;
        [Description("R/W,系统变量Bfloat94")]
        public float Bfloat94;
        [Description("R/W,系统变量Bfloat95")]
        public float Bfloat95;
        [Description("R/W,系统变量Bfloat96")]
        public float Bfloat96;
        [Description("R/W,系统变量Bfloat97")]
        public float Bfloat97;
        [Description("R/W,系统变量Bfloat98")]
        public float Bfloat98;
        [Description("R/W,系统变量Bfloat99")]
        public float Bfloat99;
        [Description("R/W,系统变量Bfloat10")]
        public float Bfloat100;
        [Description("R/W,系统变量Bfloat11")]
        public float Bfloat101;
        [Description("R/W,系统变量Bfloat12")]
        public float Bfloat102;
        [Description("R/W,系统变量Bfloat13")]
        public float Bfloat103;
        [Description("R/W,系统变量Bfloat14")]
        public float Bfloat104;
        [Description("R/W,系统变量Bfloat15")]
        public float Bfloat105;
        [Description("R/W,系统变量Bfloat16")]
        public float Bfloat106;
        [Description("R/W,系统变量Bfloat17")]
        public float Bfloat107;
        [Description("R/W,系统变量Bfloat18")]
        public float Bfloat108;
        [Description("R/W,系统变量Bfloat19")]
        public float Bfloat109;
        [Description("R/W,系统变量Bfloat110")]
        public float Bfloat110;
        [Description("R/W,系统变量Bfloat111")]
        public float Bfloat111;
        [Description("R/W,系统变量Bfloat112")]
        public float Bfloat112;
        [Description("R/W,系统变量Bfloat113")]
        public float Bfloat113;
        [Description("R/W,系统变量Bfloat114")]
        public float Bfloat114;
        [Description("R/W,系统变量Bfloat115")]
        public float Bfloat115;
        [Description("R/W,系统变量Bfloat116")]
        public float Bfloat116;
        [Description("R/W,系统变量Bfloat117")]
        public float Bfloat117;
        [Description("R/W,系统变量Bfloat118")]
        public float Bfloat118;
        [Description("R/W,系统变量Bfloat119")]
        public float Bfloat119;
        [Description("R/W,系统变量Bfloat120")]
        public float Bfloat120;
        [Description("R/W,系统变量Bfloat121")]
        public float Bfloat121;
        [Description("R/W,系统变量Bfloat122")]
        public float Bfloat122;
        [Description("R/W,系统变量Bfloat123")]
        public float Bfloat123;
        [Description("R/W,系统变量Bfloat124")]
        public float Bfloat124;
        [Description("R/W,系统变量Bfloat125")]
        public float Bfloat125;
        [Description("R/W,系统变量Bfloat126")]
        public float Bfloat126;
        [Description("R/W,系统变量Bfloat127")]
        public float Bfloat127;
        [Description("R/W,系统变量Bfloat128")]
        public float Bfloat128;
        [Description("R/W,系统变量Bfloat129")]
        public float Bfloat129;
        [Description("R/W,系统变量Bfloat130")]
        public float Bfloat130;
        [Description("R/W,系统变量Bfloat131")]
        public float Bfloat131;
        [Description("R/W,系统变量Bfloat132")]
        public float Bfloat132;
        [Description("R/W,系统变量Bfloat133")]
        public float Bfloat133;
        [Description("R/W,系统变量Bfloat134")]
        public float Bfloat134;
        [Description("R/W,系统变量Bfloat135")]
        public float Bfloat135;
        [Description("R/W,系统变量Bfloat136")]
        public float Bfloat136;
        [Description("R/W,系统变量Bfloat137")]
        public float Bfloat137;
        [Description("R/W,系统变量Bfloat138")]
        public float Bfloat138;
        [Description("R/W,系统变量Bfloat139")]
        public float Bfloat139;
        [Description("R/W,系统变量Bfloat140")]
        public float Bfloat140;
        [Description("R/W,系统变量Bfloat141")]
        public float Bfloat141;
        [Description("R/W,系统变量Bfloat142")]
        public float Bfloat142;
        [Description("R/W,系统变量Bfloat143")]
        public float Bfloat143;
        [Description("R/W,系统变量Bfloat144")]
        public float Bfloat144;
        [Description("R/W,系统变量Bfloat145")]
        public float Bfloat145;
        [Description("R/W,系统变量Bfloat146")]
        public float Bfloat146;
        [Description("R/W,系统变量Bfloat147")]
        public float Bfloat147;
        [Description("R/W,系统变量Bfloat148")]
        public float Bfloat148;
        [Description("R/W,系统变量Bfloat149")]
        public float Bfloat149;
        [Description("R/W,系统变量Bfloat150")]
        public float Bfloat150;
        [Description("R/W,系统变量Bfloat151")]
        public float Bfloat151;
        [Description("R/W,系统变量Bfloat152")]
        public float Bfloat152;
        [Description("R/W,系统变量Bfloat153")]
        public float Bfloat153;
        [Description("R/W,系统变量Bfloat154")]
        public float Bfloat154;
        [Description("R/W,系统变量Bfloat155")]
        public float Bfloat155;
        [Description("R/W,系统变量Bfloat156")]
        public float Bfloat156;
        [Description("R/W,系统变量Bfloat157")]
        public float Bfloat157;
        [Description("R/W,系统变量Bfloat158")]
        public float Bfloat158;
        [Description("R/W,系统变量Bfloat159")]
        public float Bfloat159;
        [Description("R/W,系统变量Bfloat160")]
        public float Bfloat160;
        [Description("R/W,系统变量Bfloat161")]
        public float Bfloat161;
        [Description("R/W,系统变量Bfloat162")]
        public float Bfloat162;
        [Description("R/W,系统变量Bfloat163")]
        public float Bfloat163;
        [Description("R/W,系统变量Bfloat164")]
        public float Bfloat164;
        [Description("R/W,系统变量Bfloat165")]
        public float Bfloat165;
        [Description("R/W,系统变量Bfloat166")]
        public float Bfloat166;
        [Description("R/W,系统变量Bfloat167")]
        public float Bfloat167;
        [Description("R/W,系统变量Bfloat168")]
        public float Bfloat168;
        [Description("R/W,系统变量Bfloat169")]
        public float Bfloat169;
        [Description("R/W,系统变量Bfloat170")]
        public float Bfloat170;
        [Description("R/W,系统变量Bfloat171")]
        public float Bfloat171;
        [Description("R/W,系统变量Bfloat172")]
        public float Bfloat172;
        [Description("R/W,系统变量Bfloat173")]
        public float Bfloat173;
        [Description("R/W,系统变量Bfloat174")]
        public float Bfloat174;
        [Description("R/W,系统变量Bfloat175")]
        public float Bfloat175;
        [Description("R/W,系统变量Bfloat176")]
        public float Bfloat176;
        [Description("R/W,系统变量Bfloat177")]
        public float Bfloat177;
        [Description("R/W,系统变量Bfloat178")]
        public float Bfloat178;
        [Description("R/W,系统变量Bfloat179")]
        public float Bfloat179;
        [Description("R/W,系统变量Bfloat180")]
        public float Bfloat180;
        [Description("R/W,系统变量Bfloat181")]
        public float Bfloat181;
        [Description("R/W,系统变量Bfloat182")]
        public float Bfloat182;
        [Description("R/W,系统变量Bfloat183")]
        public float Bfloat183;
        [Description("R/W,系统变量Bfloat184")]
        public float Bfloat184;
        [Description("R/W,系统变量Bfloat185")]
        public float Bfloat185;
        [Description("R/W,系统变量Bfloat186")]
        public float Bfloat186;
        [Description("R/W,系统变量Bfloat187")]
        public float Bfloat187;
        [Description("R/W,系统变量Bfloat188")]
        public float Bfloat188;
        [Description("R/W,系统变量Bfloat189")]
        public float Bfloat189;
        [Description("R/W,系统变量Bfloat190")]
        public float Bfloat190;
        [Description("R/W,系统变量Bfloat191")]
        public float Bfloat191;
        [Description("R/W,系统变量Bfloat192")]
        public float Bfloat192;
        [Description("R/W,系统变量Bfloat193")]
        public float Bfloat193;
        [Description("R/W,系统变量Bfloat194")]
        public float Bfloat194;
        [Description("R/W,系统变量Bfloat195")]
        public float Bfloat195;
        [Description("R/W,系统变量Bfloat196")]
        public float Bfloat196;
        [Description("R/W,系统变量Bfloat197")]
        public float Bfloat197;
        [Description("R/W,系统变量Bfloat198")]
        public float Bfloat198;
        [Description("R/W,系统变量Bfloat199")]
        public float Bfloat199;
        [Description("R/W,系统变量Bfloat200")]
        public float Bfloat200;
        [Description("R/W,系统变量Bfloat201")]
        public float Bfloat201;
        [Description("R/W,系统变量Bfloat202")]
        public float Bfloat202;
        [Description("R/W,系统变量Bfloat203")]
        public float Bfloat203;
        [Description("R/W,系统变量Bfloat204")]
        public float Bfloat204;
        [Description("R/W,系统变量Bfloat205")]
        public float Bfloat205;
        [Description("R/W,系统变量Bfloat206")]
        public float Bfloat206;
        [Description("R/W,系统变量Bfloat207")]
        public float Bfloat207;
        [Description("R/W,系统变量Bfloat208")]
        public float Bfloat208;
        [Description("R/W,系统变量Bfloat209")]
        public float Bfloat209;
        [Description("R/W,系统变量Bfloat210")]
        public float Bfloat210;
        [Description("R/W,系统变量Bfloat211")]
        public float Bfloat211;
        [Description("R/W,系统变量Bfloat212")]
        public float Bfloat212;
        [Description("R/W,系统变量Bfloat213")]
        public float Bfloat213;
        [Description("R/W,系统变量Bfloat214")]
        public float Bfloat214;
        [Description("R/W,系统变量Bfloat215")]
        public float Bfloat215;
        [Description("R/W,系统变量Bfloat216")]
        public float Bfloat216;
        [Description("R/W,系统变量Bfloat217")]
        public float Bfloat217;
        [Description("R/W,系统变量Bfloat218")]
        public float Bfloat218;
        [Description("R/W,系统变量Bfloat219")]
        public float Bfloat219;
        [Description("R/W,系统变量Bfloat220")]
        public float Bfloat220;
        [Description("R/W,系统变量Bfloat221")]
        public float Bfloat221;
        [Description("R/W,系统变量Bfloat222")]
        public float Bfloat222;
        [Description("R/W,系统变量Bfloat223")]
        public float Bfloat223;
        [Description("R/W,系统变量Bfloat224")]
        public float Bfloat224;
        [Description("R/W,系统变量Bfloat225")]
        public float Bfloat225;
        [Description("R/W,系统变量Bfloat226")]
        public float Bfloat226;
        [Description("R/W,系统变量Bfloat227")]
        public float Bfloat227;
        [Description("R/W,系统变量Bfloat228")]
        public float Bfloat228;
        [Description("R/W,系统变量Bfloat229")]
        public float Bfloat229;
        [Description("R/W,系统变量Bfloat230")]
        public float Bfloat230;
        [Description("R/W,系统变量Bfloat231")]
        public float Bfloat231;
        [Description("R/W,系统变量Bfloat232")]
        public float Bfloat232;
        [Description("R/W,系统变量Bfloat233")]
        public float Bfloat233;
        [Description("R/W,系统变量Bfloat234")]
        public float Bfloat234;
        [Description("R/W,系统变量Bfloat235")]
        public float Bfloat235;
        [Description("R/W,系统变量Bfloat236")]
        public float Bfloat236;
        [Description("R/W,系统变量Bfloat237")]
        public float Bfloat237;
        [Description("R/W,系统变量Bfloat238")]
        public float Bfloat238;
        [Description("R/W,系统变量Bfloat239")]
        public float Bfloat239;
        [Description("R/W,系统变量Bfloat240")]
        public float Bfloat240;
        [Description("R/W,系统变量Bfloat241")]
        public float Bfloat241;
        [Description("R/W,系统变量Bfloat242")]
        public float Bfloat242;
        [Description("R/W,系统变量Bfloat243")]
        public float Bfloat243;
        [Description("R/W,系统变量Bfloat244")]
        public float Bfloat244;
        [Description("R/W,系统变量Bfloat245")]
        public float Bfloat245;
        [Description("R/W,系统变量Bfloat246")]
        public float Bfloat246;
        [Description("R/W,系统变量Bfloat247")]
        public float Bfloat247;
        [Description("R/W,系统变量Bfloat248")]
        public float Bfloat248;
        [Description("R/W,系统变量Bfloat249")]
        public float Bfloat249;
        [Description("R/W,系统变量Bfloat250")]
        public float Bfloat250;
        [Description("R/W,系统变量Bfloat251")]
        public float Bfloat251;
        [Description("R/W,系统变量Bfloat252")]
        public float Bfloat252;
        [Description("R/W,系统变量Bfloat253")]
        public float Bfloat253;
        [Description("R/W,系统变量Bfloat254")]
        public float Bfloat254;
        [Description("R/W,系统变量Bfloat255")]
        public float Bfloat255;
        [Description("R/W,系统变量Bfloat256")]
        public float Bfloat256;
        [Description("R/W,系统变量Bfloat257")]
        public float Bfloat257;
        [Description("R/W,系统变量Bfloat258")]
        public float Bfloat258;
        [Description("R/W,系统变量Bfloat259")]
        public float Bfloat259;
        [Description("R/W,系统变量Bfloat260")]
        public float Bfloat260;
        [Description("R/W,系统变量Bfloat261")]
        public float Bfloat261;
        [Description("R/W,系统变量Bfloat262")]
        public float Bfloat262;
        [Description("R/W,系统变量Bfloat263")]
        public float Bfloat263;
        [Description("R/W,系统变量Bfloat264")]
        public float Bfloat264;
        [Description("R/W,系统变量Bfloat265")]
        public float Bfloat265;
        [Description("R/W,系统变量Bfloat266")]
        public float Bfloat266;
        [Description("R/W,系统变量Bfloat267")]
        public float Bfloat267;
        [Description("R/W,系统变量Bfloat268")]
        public float Bfloat268;
        [Description("R/W,系统变量Bfloat269")]
        public float Bfloat269;
        [Description("R/W,系统变量Bfloat270")]
        public float Bfloat270;
        [Description("R/W,系统变量Bfloat271")]
        public float Bfloat271;
        [Description("R/W,系统变量Bfloat272")]
        public float Bfloat272;
        [Description("R/W,系统变量Bfloat273")]
        public float Bfloat273;
        [Description("R/W,系统变量Bfloat274")]
        public float Bfloat274;
        [Description("R/W,系统变量Bfloat275")]
        public float Bfloat275;
        [Description("R/W,系统变量Bfloat276")]
        public float Bfloat276;
        [Description("R/W,系统变量Bfloat277")]
        public float Bfloat277;
        [Description("R/W,系统变量Bfloat278")]
        public float Bfloat278;
        [Description("R/W,系统变量Bfloat279")]
        public float Bfloat279;
        [Description("R/W,系统变量Bfloat280")]
        public float Bfloat280;
        [Description("R/W,系统变量Bfloat281")]
        public float Bfloat281;
        [Description("R/W,系统变量Bfloat282")]
        public float Bfloat282;
        [Description("R/W,系统变量Bfloat283")]
        public float Bfloat283;
        [Description("R/W,系统变量Bfloat284")]
        public float Bfloat284;
        [Description("R/W,系统变量Bfloat285")]
        public float Bfloat285;
        [Description("R/W,系统变量Bfloat286")]
        public float Bfloat286;
        [Description("R/W,系统变量Bfloat287")]
        public float Bfloat287;
        [Description("R/W,系统变量Bfloat288")]
        public float Bfloat288;
        [Description("R/W,系统变量Bfloat289")]
        public float Bfloat289;
        [Description("R/W,系统变量Bfloat290")]
        public float Bfloat290;
        [Description("R/W,系统变量Bfloat291")]
        public float Bfloat291;
        [Description("R/W,系统变量Bfloat292")]
        public float Bfloat292;
        [Description("R/W,系统变量Bfloat293")]
        public float Bfloat293;
        [Description("R/W,系统变量Bfloat294")]
        public float Bfloat294;
        [Description("R/W,系统变量Bfloat295")]
        public float Bfloat295;
        [Description("R/W,系统变量Bfloat296")]
        public float Bfloat296;
        [Description("R/W,系统变量Bfloat297")]
        public float Bfloat297;
        [Description("R/W,系统变量Bfloat298")]
        public float Bfloat298;
        [Description("R/W,系统变量Bfloat299")]
        public float Bfloat299;
        #endregion

        #region string型变量

        [Description("R/W,系统变量Bstring0")] public string Bstring0 = string.Empty;
        [Description("R/W,系统变量Bstring1")]
        public string Bstring1 = string.Empty;
        [Description("R/W,系统变量Bstring2")]
        public string Bstring2 = string.Empty;
        [Description("R/W,系统变量Bstring3")]
        public string Bstring3 = string.Empty;
        [Description("R/W,系统变量Bstring4")]
        public string Bstring4 = string.Empty;
        [Description("R/W,系统变量Bstring5")]
        public string Bstring5 = string.Empty;
        [Description("R/W,系统变量Bstring6")]
        public string Bstring6 = string.Empty;
        [Description("R/W,系统变量Bstring7")]
        public string Bstring7 = string.Empty;
        [Description("R/W,系统变量Bstring8")]
        public string Bstring8 = string.Empty;
        [Description("R/W,系统变量Bstring9")]
        public string Bstring9 = string.Empty;
        [Description("R/W,系统变量Bstring10")]
        public string Bstring10 = string.Empty;
        [Description("R/W,系统变量Bstring11")]
        public string Bstring11 = string.Empty;
        [Description("R/W,系统变量Bstring12")]
        public string Bstring12 = string.Empty;
        [Description("R/W,系统变量Bstring13")]
        public string Bstring13 = string.Empty;
        [Description("R/W,系统变量Bstring14")]
        public string Bstring14 = string.Empty;
        [Description("R/W,系统变量Bstring15")]
        public string Bstring15 = string.Empty;
        [Description("R/W,系统变量Bstring16")]
        public string Bstring16 = string.Empty;
        [Description("R/W,系统变量Bstring17")]
        public string Bstring17 = string.Empty;
        [Description("R/W,系统变量Bstring18")]
        public string Bstring18 = string.Empty;
        [Description("R/W,系统变量Bstring19")]
        public string Bstring19 = string.Empty;
        [Description("R/W,系统变量Bstring20")]
        public string Bstring20 = string.Empty;
        [Description("R/W,系统变量Bstring21")]
        public string Bstring21 = string.Empty;
        [Description("R/W,系统变量Bstring22")]
        public string Bstring22 = string.Empty;
        [Description("R/W,系统变量Bstring23")]
        public string Bstring23 = string.Empty;
        [Description("R/W,系统变量Bstring24")]
        public string Bstring24 = string.Empty;
        [Description("R/W,系统变量Bstring25")]
        public string Bstring25 = string.Empty;
        [Description("R/W,系统变量Bstring26")]
        public string Bstring26 = string.Empty;
        [Description("R/W,系统变量Bstring27")]
        public string Bstring27 = string.Empty;
        [Description("R/W,系统变量Bstring28")]
        public string Bstring28 = string.Empty;
        [Description("R/W,系统变量Bstring29")]
        public string Bstring29 = string.Empty;
        [Description("R/W,系统变量Bstring30")]
        public string Bstring30 = string.Empty;
        [Description("R/W,系统变量Bstring31")]
        public string Bstring31 = string.Empty;
        [Description("R/W,系统变量Bstring32")]
        public string Bstring32 = string.Empty;
        [Description("R/W,系统变量Bstring33")]
        public string Bstring33 = string.Empty;
        [Description("R/W,系统变量Bstring34")]
        public string Bstring34 = string.Empty;
        [Description("R/W,系统变量Bstring35")]
        public string Bstring35 = string.Empty;
        [Description("R/W,系统变量Bstring36")]
        public string Bstring36 = string.Empty;
        [Description("R/W,系统变量Bstring37")]
        public string Bstring37 = string.Empty;
        [Description("R/W,系统变量Bstring38")]
        public string Bstring38 = string.Empty;
        [Description("R/W,系统变量Bstring39")]
        public string Bstring39 = string.Empty;
        [Description("R/W,系统变量Bstring40")]
        public string Bstring40 = string.Empty;
        [Description("R/W,系统变量Bstring41")]
        public string Bstring41 = string.Empty;
        [Description("R/W,系统变量Bstring42")]
        public string Bstring42 = string.Empty;
        [Description("R/W,系统变量Bstring43")]
        public string Bstring43 = string.Empty;
        [Description("R/W,系统变量Bstring44")]
        public string Bstring44 = string.Empty;
        [Description("R/W,系统变量Bstring45")]
        public string Bstring45 = string.Empty;
        [Description("R/W,系统变量Bstring46")]
        public string Bstring46 = string.Empty;
        [Description("R/W,系统变量Bstring47")]
        public string Bstring47 = string.Empty;
        [Description("R/W,系统变量Bstring48")]
        public string Bstring48 = string.Empty;
        [Description("R/W,系统变量Bstring49")]
        public string Bstring49 = string.Empty;
        [Description("R/W,系统变量Bstring50")]
        public string Bstring50 = string.Empty;
        [Description("R/W,系统变量Bstring51")]
        public string Bstring51 = string.Empty;
        [Description("R/W,系统变量Bstring52")]
        public string Bstring52 = string.Empty;
        [Description("R/W,系统变量Bstring53")]
        public string Bstring53 = string.Empty;
        [Description("R/W,系统变量Bstring54")]
        public string Bstring54 = string.Empty;
        [Description("R/W,系统变量Bstring55")]
        public string Bstring55 = string.Empty;
        [Description("R/W,系统变量Bstring56")]
        public string Bstring56 = string.Empty;
        [Description("R/W,系统变量Bstring57")]
        public string Bstring57 = string.Empty;
        [Description("R/W,系统变量Bstring58")]
        public string Bstring58 = string.Empty;
        [Description("R/W,系统变量Bstring59")]
        public string Bstring59 = string.Empty;
        [Description("R/W,系统变量Bstring60")]
        public string Bstring60 = string.Empty;
        [Description("R/W,系统变量Bstring61")]
        public string Bstring61 = string.Empty;
        [Description("R/W,系统变量Bstring62")]
        public string Bstring62 = string.Empty;
        [Description("R/W,系统变量Bstring63")]
        public string Bstring63 = string.Empty;
        [Description("R/W,系统变量Bstring64")]
        public string Bstring64 = string.Empty;
        [Description("R/W,系统变量Bstring65")]
        public string Bstring65 = string.Empty;
        [Description("R/W,系统变量Bstring66")]
        public string Bstring66 = string.Empty;
        [Description("R/W,系统变量Bstring67")]
        public string Bstring67 = string.Empty;
        [Description("R/W,系统变量Bstring68")]
        public string Bstring68 = string.Empty;
        [Description("R/W,系统变量Bstring69")]
        public string Bstring69 = string.Empty;
        [Description("R/W,系统变量Bstring70")]
        public string Bstring70 = string.Empty;
        [Description("R/W,系统变量Bstring71")]
        public string Bstring71 = string.Empty;
        [Description("R/W,系统变量Bstring72")]
        public string Bstring72 = string.Empty;
        [Description("R/W,系统变量Bstring73")]
        public string Bstring73 = string.Empty;
        [Description("R/W,系统变量Bstring74")]
        public string Bstring74 = string.Empty;
        [Description("R/W,系统变量Bstring75")]
        public string Bstring75 = string.Empty;
        [Description("R/W,系统变量Bstring76")]
        public string Bstring76;
        [Description("R/W,系统变量Bstring77")]
        public string Bstring77;
        [Description("R/W,系统变量Bstring78")]
        public string Bstring78;
        [Description("R/W,系统变量Bstring79")]
        public string Bstring79;
        [Description("R/W,系统变量Bstring80")]
        public string Bstring80;
        [Description("R/W,系统变量Bstring81")]
        public string Bstring81;
        [Description("R/W,系统变量Bstring82")]
        public string Bstring82;
        [Description("R/W,系统变量Bstring83")]
        public string Bstring83;
        [Description("R/W,系统变量Bstring84")]
        public string Bstring84;
        [Description("R/W,系统变量Bstring85")]
        public string Bstring85;
        [Description("R/W,系统变量Bstring86")]
        public string Bstring86;
        [Description("R/W,系统变量Bstring87")]
        public string Bstring87;
        [Description("R/W,系统变量Bstring88")]
        public string Bstring88;
        [Description("R/W,系统变量Bstring89")]
        public string Bstring89;
        [Description("R/W,系统变量Bstring90")]
        public string Bstring90;
        [Description("R/W,系统变量Bstring91")]
        public string Bstring91;
        [Description("R/W,系统变量Bstring92")]
        public string Bstring92;
        [Description("R/W,系统变量Bstring93")]
        public string Bstring93;
        [Description("R/W,系统变量Bstring94")]
        public string Bstring94;
        [Description("R/W,系统变量Bstring95")]
        public string Bstring95;
        [Description("R/W,系统变量Bstring96")]
        public string Bstring96;
        [Description("R/W,系统变量Bstring97")]
        public string Bstring97;
        [Description("R/W,系统变量Bstring98")]
        public string Bstring98;
        [Description("R/W,系统变量Bstring99")]
        public string Bstring99;
        [Description("R/W,系统变量Bstring100")]
        public string Bstring100;
        [Description("R/W,系统变量Bstring101")]
        public string Bstring101;
        [Description("R/W,系统变量Bstring102")]
        public string Bstring102;
        [Description("R/W,系统变量Bstring103")]
        public string Bstring103;
        [Description("R/W,系统变量Bstring104")]
        public string Bstring104;
        [Description("R/W,系统变量Bstring105")]
        public string Bstring105;
        [Description("R/W,系统变量Bstring106")]
        public string Bstring106;
        [Description("R/W,系统变量Bstring107")]
        public string Bstring107;
        [Description("R/W,系统变量Bstring108")]
        public string Bstring108;
        [Description("R/W,系统变量Bstring109")]
        public string Bstring109;
        [Description("R/W,系统变量Bstring110")]
        public string Bstring110;
        [Description("R/W,系统变量Bstring111")]
        public string Bstring111;
        [Description("R/W,系统变量Bstring112")]
        public string Bstring112;
        [Description("R/W,系统变量Bstring113")]
        public string Bstring113;
        [Description("R/W,系统变量Bstring114")]
        public string Bstring114;
        [Description("R/W,系统变量Bstring115")]
        public string Bstring115;
        [Description("R/W,系统变量Bstring116")]
        public string Bstring116;
        [Description("R/W,系统变量Bstring117")]
        public string Bstring117;
        [Description("R/W,系统变量Bstring118")]
        public string Bstring118;
        [Description("R/W,系统变量Bstring119")]
        public string Bstring119;
        [Description("R/W,系统变量Bstring120")]
        public string Bstring120;
        [Description("R/W,系统变量Bstring121")]
        public string Bstring121;
        [Description("R/W,系统变量Bstring122")]
        public string Bstring122;
        [Description("R/W,系统变量Bstring123")]
        public string Bstring123;
        [Description("R/W,系统变量Bstring124")]
        public string Bstring124;
        [Description("R/W,系统变量Bstring125")]
        public string Bstring125;
        [Description("R/W,系统变量Bstring126")]
        public string Bstring126;
        [Description("R/W,系统变量Bstring127")]
        public string Bstring127;
        [Description("R/W,系统变量Bstring128")]
        public string Bstring128;
        [Description("R/W,系统变量Bstring129")]
        public string Bstring129;
        [Description("R/W,系统变量Bstring130")]
        public string Bstring130;
        [Description("R/W,系统变量Bstring131")]
        public string Bstring131;
        [Description("R/W,系统变量Bstring132")]
        public string Bstring132;
        [Description("R/W,系统变量Bstring133")]
        public string Bstring133;
        [Description("R/W,系统变量Bstring134")]
        public string Bstring134;
        [Description("R/W,系统变量Bstring135")]
        public string Bstring135;
        [Description("R/W,系统变量Bstring136")]
        public string Bstring136;
        [Description("R/W,系统变量Bstring137")]
        public string Bstring137;
        [Description("R/W,系统变量Bstring138")]
        public string Bstring138;
        [Description("R/W,系统变量Bstring139")]
        public string Bstring139;
        [Description("R/W,系统变量Bstring140")]
        public string Bstring140;
        [Description("R/W,系统变量Bstring141")]
        public string Bstring141;
        [Description("R/W,系统变量Bstring142")]
        public string Bstring142;
        [Description("R/W,系统变量Bstring143")]
        public string Bstring143;
        [Description("R/W,系统变量Bstring144")]
        public string Bstring144;
        [Description("R/W,系统变量Bstring145")]
        public string Bstring145;
        [Description("R/W,系统变量Bstring146")]
        public string Bstring146;
        [Description("R/W,系统变量Bstring147")]
        public string Bstring147;
        [Description("R/W,系统变量Bstring148")]
        public string Bstring148;
        [Description("R/W,系统变量Bstring149")]
        public string Bstring149;
        [Description("R/W,系统变量Bstring150")]
        public string Bstring150;
        [Description("R/W,系统变量Bstring151")]
        public string Bstring151;
        [Description("R/W,系统变量Bstring152")]
        public string Bstring152;
        [Description("R/W,系统变量Bstring153")]
        public string Bstring153;
        [Description("R/W,系统变量Bstring154")]
        public string Bstring154;
        [Description("R/W,系统变量Bstring155")]
        public string Bstring155;
        [Description("R/W,系统变量Bstring156")]
        public string Bstring156;
        [Description("R/W,系统变量Bstring157")]
        public string Bstring157;
        [Description("R/W,系统变量Bstring158")]
        public string Bstring158;
        [Description("R/W,系统变量Bstring159")]
        public string Bstring159;
        [Description("R/W,系统变量Bstring160")]
        public string Bstring160;
        [Description("R/W,系统变量Bstring161")]
        public string Bstring161;
        [Description("R/W,系统变量Bstring162")]
        public string Bstring162;
        [Description("R/W,系统变量Bstring163")]
        public string Bstring163;
        [Description("R/W,系统变量Bstring164")]
        public string Bstring164;
        [Description("R/W,系统变量Bstring165")]
        public string Bstring165;
        [Description("R/W,系统变量Bstring166")]
        public string Bstring166;
        [Description("R/W,系统变量Bstring167")]
        public string Bstring167;
        [Description("R/W,系统变量Bstring168")]
        public string Bstring168;
        [Description("R/W,系统变量Bstring169")]
        public string Bstring169;
        [Description("R/W,系统变量Bstring170")]
        public string Bstring170;
        [Description("R/W,系统变量Bstring171")]
        public string Bstring171;
        [Description("R/W,系统变量Bstring172")]
        public string Bstring172;
        [Description("R/W,系统变量Bstring173")]
        public string Bstring173;
        [Description("R/W,系统变量Bstring174")]
        public string Bstring174;
        [Description("R/W,系统变量Bstring175")]
        public string Bstring175;
        [Description("R/W,系统变量Bstring176")]
        public string Bstring176;
        [Description("R/W,系统变量Bstring177")]
        public string Bstring177;
        [Description("R/W,系统变量Bstring178")]
        public string Bstring178;
        [Description("R/W,系统变量Bstring179")]
        public string Bstring179;
        [Description("R/W,系统变量Bstring180")]
        public string Bstring180;
        [Description("R/W,系统变量Bstring181")]
        public string Bstring181;
        [Description("R/W,系统变量Bstring182")]
        public string Bstring182;
        [Description("R/W,系统变量Bstring183")]
        public string Bstring183;
        [Description("R/W,系统变量Bstring184")]
        public string Bstring184;
        [Description("R/W,系统变量Bstring185")]
        public string Bstring185;
        [Description("R/W,系统变量Bstring186")]
        public string Bstring186;
        [Description("R/W,系统变量Bstring187")]
        public string Bstring187;
        [Description("R/W,系统变量Bstring188")]
        public string Bstring188;
        [Description("R/W,系统变量Bstring189")]
        public string Bstring189;
        [Description("R/W,系统变量Bstring190")]
        public string Bstring190;
        [Description("R/W,系统变量Bstring191")]
        public string Bstring191;
        [Description("R/W,系统变量Bstring192")]
        public string Bstring192;
        [Description("R/W,系统变量Bstring193")]
        public string Bstring193;
        [Description("R/W,系统变量Bstring194")]
        public string Bstring194;
        [Description("R/W,系统变量Bstring195")]
        public string Bstring195;
        [Description("R/W,系统变量Bstring196")]
        public string Bstring196;
        [Description("R/W,系统变量Bstring197")]
        public string Bstring197;
        [Description("R/W,系统变量Bstring198")]
        public string Bstring198;
        [Description("R/W,系统变量Bstring199")]
        public string Bstring199;
        [Description("R/W,系统变量Bstring200")]
        public string Bstring200;
        [Description("R/W,系统变量Bstring201")]
        public string Bstring201;
        [Description("R/W,系统变量Bstring202")]
        public string Bstring202;
        [Description("R/W,系统变量Bstring203")]
        public string Bstring203;
        [Description("R/W,系统变量Bstring204")]
        public string Bstring204;
        [Description("R/W,系统变量Bstring205")]
        public string Bstring205;
        [Description("R/W,系统变量Bstring206")]
        public string Bstring206;
        [Description("R/W,系统变量Bstring207")]
        public string Bstring207;
        [Description("R/W,系统变量Bstring208")]
        public string Bstring208;
        [Description("R/W,系统变量Bstring209")]
        public string Bstring209;
        [Description("R/W,系统变量Bstring210")]
        public string Bstring210;
        [Description("R/W,系统变量Bstring211")]
        public string Bstring211;
        [Description("R/W,系统变量Bstring212")]
        public string Bstring212;
        [Description("R/W,系统变量Bstring213")]
        public string Bstring213;
        [Description("R/W,系统变量Bstring214")]
        public string Bstring214;
        [Description("R/W,系统变量Bstring215")]
        public string Bstring215;
        [Description("R/W,系统变量Bstring216")]
        public string Bstring216;
        [Description("R/W,系统变量Bstring217")]
        public string Bstring217;
        [Description("R/W,系统变量Bstring218")]
        public string Bstring218;
        [Description("R/W,系统变量Bstring219")]
        public string Bstring219;
        [Description("R/W,系统变量Bstring220")]
        public string Bstring220;
        [Description("R/W,系统变量Bstring221")]
        public string Bstring221;
        [Description("R/W,系统变量Bstring222")]
        public string Bstring222;
        [Description("R/W,系统变量Bstring223")]
        public string Bstring223;
        [Description("R/W,系统变量Bstring224")]
        public string Bstring224;
        [Description("R/W,系统变量Bstring225")]
        public string Bstring225;
        [Description("R/W,系统变量Bstring226")]
        public string Bstring226;
        [Description("R/W,系统变量Bstring227")]
        public string Bstring227;
        [Description("R/W,系统变量Bstring228")]
        public string Bstring228;
        [Description("R/W,系统变量Bstring229")]
        public string Bstring229;
        [Description("R/W,系统变量Bstring230")]
        public string Bstring230;
        [Description("R/W,系统变量Bstring231")]
        public string Bstring231;
        [Description("R/W,系统变量Bstring232")]
        public string Bstring232;
        [Description("R/W,系统变量Bstring233")]
        public string Bstring233;
        [Description("R/W,系统变量Bstring234")]
        public string Bstring234;
        [Description("R/W,系统变量Bstring235")]
        public string Bstring235;
        [Description("R/W,系统变量Bstring236")]
        public string Bstring236;
        [Description("R/W,系统变量Bstring237")]
        public string Bstring237;
        [Description("R/W,系统变量Bstring238")]
        public string Bstring238;
        [Description("R/W,系统变量Bstring239")]
        public string Bstring239;
        [Description("R/W,系统变量Bstring240")]
        public string Bstring240;
        [Description("R/W,系统变量Bstring241")]
        public string Bstring241;
        [Description("R/W,系统变量Bstring242")]
        public string Bstring242;
        [Description("R/W,系统变量Bstring243")]
        public string Bstring243;
        [Description("R/W,系统变量Bstring244")]
        public string Bstring244;
        [Description("R/W,系统变量Bstring245")]
        public string Bstring245;
        [Description("R/W,系统变量Bstring246")]
        public string Bstring246;
        [Description("R/W,系统变量Bstring247")]
        public string Bstring247;
        [Description("R/W,系统变量Bstring248")]
        public string Bstring248;
        [Description("R/W,系统变量Bstring249")]
        public string Bstring249;
        [Description("R/W,系统变量Bstring250")]
        public string Bstring250;
        [Description("R/W,系统变量Bstring251")]
        public string Bstring251;
        [Description("R/W,系统变量Bstring252")]
        public string Bstring252;
        [Description("R/W,系统变量Bstring253")]
        public string Bstring253;
        [Description("R/W,系统变量Bstring254")]
        public string Bstring254;
        [Description("R/W,系统变量Bstring255")]
        public string Bstring255;
        [Description("R/W,系统变量Bstring256")]
        public string Bstring256;
        [Description("R/W,系统变量Bstring257")]
        public string Bstring257;
        [Description("R/W,系统变量Bstring258")]
        public string Bstring258;
        [Description("R/W,系统变量Bstring259")]
        public string Bstring259;
        [Description("R/W,系统变量Bstring260")]
        public string Bstring260;
        [Description("R/W,系统变量Bstring261")]
        public string Bstring261;
        [Description("R/W,系统变量Bstring262")]
        public string Bstring262;
        [Description("R/W,系统变量Bstring263")]
        public string Bstring263;
        [Description("R/W,系统变量Bstring264")]
        public string Bstring264;
        [Description("R/W,系统变量Bstring265")]
        public string Bstring265;
        [Description("R/W,系统变量Bstring266")]
        public string Bstring266;
        [Description("R/W,系统变量Bstring267")]
        public string Bstring267;
        [Description("R/W,系统变量Bstring268")]
        public string Bstring268;
        [Description("R/W,系统变量Bstring269")]
        public string Bstring269;
        [Description("R/W,系统变量Bstring270")]
        public string Bstring270;
        [Description("R/W,系统变量Bstring271")]
        public string Bstring271;
        [Description("R/W,系统变量Bstring272")]
        public string Bstring272;
        [Description("R/W,系统变量Bstring273")]
        public string Bstring273;
        [Description("R/W,系统变量Bstring274")]
        public string Bstring274;
        [Description("R/W,系统变量Bstring275")]
        public string Bstring275;
        [Description("R/W,系统变量Bstring276")]
        public string Bstring276;
        [Description("R/W,系统变量Bstring277")]
        public string Bstring277;
        [Description("R/W,系统变量Bstring278")]
        public string Bstring278;
        [Description("R/W,系统变量Bstring279")]
        public string Bstring279;
        [Description("R/W,系统变量Bstring280")]
        public string Bstring280;
        [Description("R/W,系统变量Bstring281")]
        public string Bstring281;
        [Description("R/W,系统变量Bstring282")]
        public string Bstring282;
        [Description("R/W,系统变量Bstring283")]
        public string Bstring283;
        [Description("R/W,系统变量Bstring284")]
        public string Bstring284;
        [Description("R/W,系统变量Bstring285")]
        public string Bstring285;
        [Description("R/W,系统变量Bstring286")]
        public string Bstring286;
        [Description("R/W,系统变量Bstring287")]
        public string Bstring287;
        [Description("R/W,系统变量Bstring288")]
        public string Bstring288;
        [Description("R/W,系统变量Bstring289")]
        public string Bstring289;
        [Description("R/W,系统变量Bstring290")]
        public string Bstring290;
        [Description("R/W,系统变量Bstring291")]
        public string Bstring291;
        [Description("R/W,系统变量Bstring292")]
        public string Bstring292;
        [Description("R/W,系统变量Bstring293")]
        public string Bstring293;
        [Description("R/W,系统变量Bstring294")]
        public string Bstring294;
        [Description("R/W,系统变量Bstring295")]
        public string Bstring295;
        [Description("R/W,系统变量Bstring296")]
        public string Bstring296;
        [Description("R/W,系统变量Bstring297")]
        public string Bstring297;
        [Description("R/W,系统变量Bstring298")]
        public string Bstring298;
        [Description("R/W,系统变量Bstring299")]
        public string Bstring299;
        #endregion

        public CheckApp(string name) :
            base(name)
        { }

        [Description("播放系统报警提示音")]
        private void Alert(string ms)
        {
            CommonMethods.MessageBeep(uint.Parse(ms));
        }

        private readonly MediaPlayer.MediaPlayer _mp = new MediaPlayer.MediaPlayer();

        [Description("播放报警声")]
        public void Alarm()
        {
            lock (_mp)
            {
                _mp.FileName = Directory.GetCurrentDirectory() + @"\Alarm.mp3";
                _mp.Play();
            }
        }

        public void Sleep(int ms)
        {
            Thread.Sleep(ms);
        }

        public void ShowConfirmForm(string showValue)
        {
            YesOrNo = string.Empty;
            YesOrNo = MessageBox.Show(showValue, @"确认", MessageBoxButtons.YesNo).ToString();
        }

        #region vision analysis buff

        public static Dictionary<string, string> ImgBuffer = new Dictionary<string, string>();

        #endregion
    }
}
