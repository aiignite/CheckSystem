using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace Controller
{
    [Description("CAN-Product,EP33-ISC-后灯")]
    public sealed class Ep33IscRearLamp : ControllerBase
    {
        #region public fields
        public CanBus CanFd;

        [Description("R,软件版本号读取")]
        public string SoftwareVersion = string.Empty;

        [Description("R,硬件版本号读取")]
        public string HardwareVersion = string.Empty;

        [Description("R,故障信息读取")]
        public string FaultRead = string.Empty;
        #endregion

        #region 构造与释放
        public Ep33IscRearLamp(string name)
            : base(name)
        {
            if (MainWorkThread != null)
            {
                MainWorkThread.Abort();
                MainWorkThread.Join();
            }

            MainWorkThread =
                new Thread(MainWork) { IsBackground = true };
            MainWorkThread.Start();
        }

        ~Ep33IscRearLamp()
        {
            Dispose();
        }
        #endregion

        #region 内部方法
        private Thread MainWorkThread { get; set; }
        private int SendCount { get; set; }
        private int SendGroupIndex { get; set; }
        private bool _isSleep = true;
        private readonly CanBus.CanDataPackage _controlDataPackage =
           new CanBus.CanDataPackage(0xFF, CanBus.CanProtocol.CanFd, CanBus.CanType.Standard, CanBus.CanFormat.Data,
               new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
        private readonly List<CanBus.CanDataPackage> _ledDataPackages =
            new List<CanBus.CanDataPackage>();
        private readonly List<int> _redLedList = new List<int>();
        private readonly List<int> _yellowLedList = new List<int>();
        private readonly List<int> _leftYellowLedList = new List<int>();
        private readonly List<int> _rightYellowLedList = new List<int>();
        private readonly List<uint> _canFdResponseIdList = new List<uint>();
        private uint _requestCanId;
        private uint _responseCanId;

        private void MainWork()
        {
            while (MainWorkThread.IsAlive)
            {
                if (!MainWorkThread.IsAlive)
                    break;

                Thread.Sleep(10);

                if (SendCount > 5)
                    SendCount = 0;
                SendCount++;

                try
                {
                    lock (_ledDataPackages)
                    {
                        if (CanFd == null || _isSleep || !_ledDataPackages.Any())
                            continue;

                        var sendPackage = new List<CanBus.CanDataPackage>();

                        if (SendCount == 1)
                        {
                            if (_isHardwareControl)
                            {
                                _controlDataPackage.CanData[0] = 0x01;
                            }
                            else
                            {
                                _controlDataPackage.CanData[0] = 0x00;
                            }

                            sendPackage.Add(_controlDataPackage);
                        }

                        if (!_isHardwareControl)
                        {
                            sendPackage.Add(_ledDataPackages[SendGroupIndex]);
                        }
                       
                        SendGroupIndex++;
                        if (SendGroupIndex == _ledDataPackages.Count)
                            SendGroupIndex = 0;

                        CanFd.SendCanDatas(sendPackage.ToArray());
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private bool _isHardwareControl;

        public void HardwareControl(bool isOn)
        {
            _isHardwareControl = isOn;
        }

        #endregion

        #region 切换灯种
        [Description("切换成R_B")]
        public void ChangeToRclb()
        {
            lock (_ledDataPackages)
            {
                _canFdResponseIdList.Clear();
                for (uint i = 0x6A1; i <= 0x6AF; i++)
                    _canFdResponseIdList.Add(i);

                _requestCanId = 0x787;
                _responseCanId = 0x797;

                const string redStr =
                    "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,86,87,88,89,90,91,92,93,94,95,96,97,98,99,100,101,102,104,106,108,110,112,114,116,118,120,122,124,125,126,127,128,129,130,131,132,133,134,135,136,137,138,139,140,141,142,143,144,145,146,147,148,149,150,151,152,153,154,155,156,157,158,159,160,161,162,163,164,165,166,167,168,169,170,171,172,173,174,175,176,177,178,179,180,182,184,186,188,190,192,194,196,198,200,202,203,204,206,208,210,212,214,216,218,220,222,224,226,228,230,232,234,236,238,239,240,241,242,243,244,245,246,247,248,249,250,251,252,253,254,255,256,257,258,259,260,261,262,263,264,265,266,267,268,269,270,271,272,273,274,275,276,277,278,279,280,281,282,283,284,285,286,287,288,289,290,291,292,293,294,295,297,299,301,303,305,307,309,311,313,315,317,319,321,323,325,327,329,330,331,333,335,337,339,341,343,345,347,349,351,353,355,357,359,361,363,365,367,369,370,371,372,373,374,375,376,377,378,379,380,381,382,383,384,385,386,387,388,389,390,391,392,393,394,395,396,397,398,399,400,401,402,403,404,405,406,407,408,409,410,411,412,413,414,415,416,417,418,419,420,421,422,423,424,425,427,429,431,433,435,437,439,441,443,445,447,449,451,453,455,457,459,461,,,463,464,,465,467,469,471,473,475,477,479,481,483,485,487,489,491,493,495,497,499,501,503,505,506,507,508,509,510,511,512,513,514,515,516,517,518,519,520,521,522,523,524,525,526,527,528,529,530,531,532,533,534,535,536,537,538,539,540,541,542,543,544,545,546,547,548,549,550,551,552,553,554,555,556,557,558,559,560,561,562,564,566,568,570,572,574,576,578,580,582,584,586,588,590,592,594,596,598,600,,602,603,604,606,608,610,612,614,616,618,620,622,624,626,628,630,632,634,636,638,640,642,644,646,647,648,649,650,651,652,653,654,655,656,657,658,659,660,661,662,663,664,665,666,667,668,669,670,671,672,673,674,675,676,677,678,679,680,681,682,683,684,685,686,687,688,689,690,691,692,693,694,695,696,697,698,699,700,701,702,704,706,708,710,712,714,716,718,720,722,724,726,728,730,732,734,736,738,740,742,,744,745,746,747,748,749,750,751,752,753,,754,756,758,760,762,764,766,768,770,772,774,776,778,780,782,784,786,788,790,792,794,796,797,798,799,800,801,802,803,804,805,806,807,808,809,810,811,812,813,814,815,816,817,818,819,820,821,822,823,824,825,826,827,828,829,830,831,832,833,834,835,836,837,838,839,840,841,842,843,844,845,846,847,848,849,850,851,852,853,855,857,859,861,863,865,867,869,871,873,875,877,879,881,883,885,887,889,891,893,,895,896,897,898,899,900,901,902,903,904,906,908,910,912,914,916,918,920,922,924,926,928,930,932,934,936,938,940,942,944,946,948,950,951,952,953,954,955,956,957,958,959,960,961,962,963,964,965,966,967,968,969,970,971,972,973,974,975,976,977,978,979,980,981,982,983,984,985,986,987,988,989,990,991,992,993,994,995,996,997,998,999,1000,1001,1002,1003,1004,1005,1006,1008,1010,1012,1014,1016,1018,1020,1022,1024,1026,1028,1030,1032,1034,1036,1038,1040,1042,1044,1046,1048,1050,1052,1054,1056,1058,1060,1062,1064,1066,1068,1070,1072,1074,1076,1078,1080,1082,1084,1086,1088,1090,1092,1094,1096,1098,1099,1100,1101,1102,1103,1104,1105,1106,1107,1108,1109,1110,1111,1112,1113,1114,1115,1116,1117,1118,1119,1120,1121,1122,1123,1124,1125,1126,1127,1128,1129,1130,1131,1132,1133,1134,1135,1136,1137,1138,1139,1140,1141,1142,1143,1144,1145,1146,1147,1148,1149,1150,1151,1152,1153,1154,1155,1157,1159,1161,1163,1165,1167,1169,1171,1173,1175,1177,1179,1181,1183,1185,1187,1189,1191,1193,1195,1197,1199,1201,1203,1205,1207,1209,1211,1213,1215,1217,1219,1221,1223,1225,1227,1229,1231,1233,1235,1237,1239,1241,1243,1245,1247,1249,1251,1253,1255,1257,1259,1260,1261,1262,1263,1264,1265,1266,1267,1268,1269,1270,1271,1272,1273,1274,1275,1276,1277,1278,1279,1280,1281,1282,1283,1284,1285,1286,1287,1288,1289,1290,1291,1292,1293,1294,1295,1296,1297,1298,1299,1300,1301,1302,1303,1304,1305,1306,1307,1308,1309,1310,1311,1312,1313,1314,1315,1317,1319,1321,1323,1325,1327,1329,1331,1333,1335,1337,1339,1341,1343,1345,1347,1349,1351,1353,1355,1357,1359,1361,1363,1365,1367,1369,1371,1373,1375,1377,1379,1381,1383,1385,1387,1389,1391,1393,1395,1397,1399,1401,1403,1405,1407,1409,1411,1413,1415,1417,1419,1421,1423,1425,1427,1429,1431,1433,1435,1436,1437,1438,1439,1440,1441,1442,1443,1444,1445,1446,1447,1448,1449,1450,1451,1452,1453,1454,1455,1456,1457,1458,1459,1460,1461,1462,1463,1464,1465,1466,1467,1468,1469,1470,1471,1472,1473,1474,1475,1476,1477,1478,1479,1480,1481,1482,1483,1484,1485,1486,1487,1488,1489,1490,1491,1492,1494,1496,1498,1500,1502,1504,1506,1508,1510,1512,1514,1516,1518,1520,1522,1524,1526,1528,1530,1532,1534,1536,1538,1540,1542,1544,1546,1548,1550,1552,1554,1556,1558,1560,1562,1564,1566,1568,1570,1572,1574,1576,1578,1580,1582,1584,1586,1588,1590,1592,1594,1596,1598,1600,1602,1604,1606,1608,1610,1612,1614,1616,1618,1619,1620,1621,1622,1623,1624,1625,1626,1627,1628,1629,1630,1631,1632,1633,1634,1635,1636,1637,1638,1639,1640,1641,1642,1643,1644,1645,1646,1647,1648,1649,1650,1651,1652,1653,1654,1655,1656,1657,1658,1659,1660,1661,1662,1663,1664,1665,1666,1667,1668,1669,1670,1671,1672,1673,1674,1676,1678,1680,1682,1684,1686,1688,1690,1692,1694,1696,1698,1700,1702,1704,1706,1708,1710,1712,1714,1716,1718,1720,1722,1724,1726,1728,1730,1732,1734,1736,1738,1740,1742,1744,1746,1748,1750,1752,1754,1756,1758,1760,1762,1764,1766,1768,1770,1772,1774,1776,1778,1780,1782,1784,1786,1788,1790,1792,1794,1796,1798,1800,1801,1802,1803,1804,1805,1806,1807,1808,1809,1810,1811,1812,1813,1814,1815,1816,1817,1818,1819,1820,1821,1822,1823,1824,1825,1826,1827,1828,1829,1830,1831,1832,1833,1834,1835,1836,1837,1838,1839,1840,1841,1842,1843,1844,1845,1846,1847,1848,1849,1850,1851,1852,1853,1854,1855,1856,1857,1859,1861,1863,1865,1867,1869,1871,1873,1875,1877,1879,1881,1883,1885,1887,1889,1891,1893,1895,1897,1899,1901,1903,1905,1907,1909,1911,1913,1915,1917,1919,1921,1923,1925,1927,1929,1931,1933,1935,1937,1939,1941,1943,1945,1947,1949,1951,1953,1955,1957,1959,1961,1963,1965,1967,1969,1971,1973,1975,1977,1979,1981,1983,1984,1985,1986,1987,1988,1989,1990,1991,1992,1993,1994,1995,1996,1997,1998,1999,2000,2001,2002,2003,2004,2005,2006,2007,2008,2009,2010,2011,2012,2013,2014,2015,2016,2017,2018,2019,2020,2021,2022,2023,2024,2025,2026,2027,2028,2029,2030,2031,2032,2033,2034,2035,2036,2037,2038,2039,2041,2043,2045,2047,2049,2051,2053,2055,2057,2059,2061,2063,2065,2067,2069,2071,2073,2075,2077,2079,2081,2083,2085,2087,2089,2091,2093,2095,2097,2099,2101,2103,2105,2107,2109,2111,2113,2115,2117,2119,2121,2123,2125,2127,2129,2131,2133,2135,2137,2139,2141,2143,2145,2147,2149,2151,2153,2155,2157,2159,2161,2163,2165,2166,2167,2168,2169,2170,2171,2172,2173,2174,2175,2176,2177,2178,2179,2180,2181,2182,2183,2184,2185,2186,2187,2188,2189,2190,2191,2192,2193,2194,2195,2196,2197,2198,2199,2200,2201,2202,2203,2204,2205,2206,2207,2208,2209,2210,2211,2212,2213,2214,2215,2216,2217,2218,2219,2220,2221,2222,2224,2226,2228,2230,2232,2234,2236,2238,2240,2242,2244,2246,2248,2250,2252,2254,2256,2258,2260,2262,2264,2266,2268,2270,2272,2274,2276,2278,2280,2282,2284,2286,2288,2290,2292,2294,2296,2298,2300,2302,2304,2306,2308,2310,2312,2314,2316,2318,2320,2322,2324,2326,2328,2330,2332,2334,2336,2338,2340,2342,2344,2346,2347,2348,2349,2350,2351,2352,2353,2354,2355,2356,2357,2358,2359,2360,2361,2362,2363,2364,2365,2366,2367,2368,2369,2370,2371,2372,2373,2374,2375,2376,2377,2378,2379,2380,2381,2382,2383,2384,2385,2386,2387,2388,2389,2390,2391,2392,2393,2394,2395,2396,2397,2398,2399,2400,2401,2402,2404,2406,2408,2410,2412,2414,2416,2418,2420,2422,2424,2426,2428,2430,2432,2434,2436,2438,2440,2442,2444,2446,2448,2450,2452,2454,2456,2458,2460,2462";
                const string yellowStr =
                    "103,105,107,109,111,113,115,117,119,121,123,181,183,185,187,189,191,193,195,197,199,201,205,207,209,211,213,215,217,219,221,223,225,227,229,231,233,235,237,296,298,300,302,304,306,308,310,312,314,316,318,320,322,324,326,328,332,334,336,338,340,342,344,346,348,350,352,354,356,358,360,362,364,366,368,426,428,430,432,434,436,438,440,442,444,446,448,450,452,454,456,458,460,462,466,468,470,472,474,476,478,480,482,484,486,488,490,492,494,496,498,500,502,504,563,565,567,569,571,573,575,577,579,581,583,585,587,589,591,593,595,597,599,601,605,607,609,611,613,615,617,619,621,623,625,627,629,631,633,635,637,639,641,643,645,703,705,707,709,711,713,715,717,719,721,723,725,727,729,731,733,735,737,739,741,743,755,757,759,761,763,765,767,769,771,773,775,777,779,781,783,785,787,789,791,793,795,854,856,858,860,862,864,866,868,870,872,874,876,878,880,882,884,886,888,890,892,894,905,907,909,911,913,915,917,919,921,923,925,927,929,931,933,935,937,939,941,943,945,947,949,1007,1009,1011,1013,1015,1017,1019,1021,1023,1025,1027,1029,1031,1033,1035,1037,1039,1041,1043,1045,1047,1049,1051,1053,1055,1057,1059,1061,1063,1065,1067,1069,1071,1073,1075,1077,1079,1081,1083,1085,1087,1089,1091,1093,1095,1097,1156,1158,1160,1162,1164,1166,1168,1170,1172,1174,1176,1178,1180,1182,1184,1186,1188,1190,1192,1194,1196,1198,1200,1202,1204,1206,1208,1210,1212,1214,1216,1218,1220,1222,1224,1226,1228,1230,1232,1234,1236,1238,1240,1242,1244,1246,1248,1250,1252,1254,1256,1258,1316,1318,1320,1322,1324,1326,1328,1330,1332,1334,1336,1338,1340,1342,1344,1346,1348,1350,1352,1354,1356,1358,1360,1362,1364,1366,1368,1370,1372,1374,1376,1378,1380,1382,1384,1386,1388,1390,1392,1394,1396,1398,1400,1402,1404,1406,1408,1410,1412,1414,1416,1418,1420,1422,1424,1426,1428,1430,1432,1434,1493,1495,1497,1499,1501,1503,1505,1507,1509,1511,1513,1515,1517,1519,1521,1523,1525,1527,1529,1531,1533,1535,1537,1539,1541,1543,1545,1547,1549,1551,1553,1555,1557,1559,1561,1563,1565,1567,1569,1571,1573,1575,1577,1579,1581,1583,1585,1587,1589,1591,1593,1595,1597,1599,1601,1603,1605,1607,1609,1611,1613,1615,1617,1675,1677,1679,1681,1683,1685,1687,1689,1691,1693,1695,1697,1699,1701,1703,1705,1707,1709,1711,1713,1715,1717,1719,1721,1723,1725,1727,1729,1731,1733,1735,1737,1739,1741,1743,1745,1747,1749,1751,1753,1755,1757,1759,1761,1763,1765,1767,1769,1771,1773,1775,1777,1779,1781,1783,1785,1787,1789,1791,1793,1795,1797,1799,1858,1860,1862,1864,1866,1868,1870,1872,1874,1876,1878,1880,1882,1884,1886,1888,1890,1892,1894,1896,1898,1900,1902,1904,1906,1908,1910,1912,1914,1916,1918,1920,1922,1924,1926,1928,1930,1932,1934,1936,1938,1940,1942,1944,1946,1948,1950,1952,1954,1956,1958,1960,1962,1964,1966,1968,1970,1972,1974,1976,1978,1980,1982,2040,2042,2044,2046,2048,2050,2052,2054,2056,2058,2060,2062,2064,2066,2068,2070,2072,2074,2076,2078,2080,2082,2084,2086,2088,2090,2092,2094,2096,2098,2100,2102,2104,2106,2108,2110,2112,2114,2116,2118,2120,2122,2124,2126,2128,2130,2132,2134,2136,2138,2140,2142,2144,2146,2148,2150,2152,2154,2156,2158,2160,2162,2164,2223,2225,2227,2229,2231,2233,2235,2237,2239,2241,2243,2245,2247,2249,2251,2253,2255,2257,2259,2261,2263,2265,2267,2269,2271,2273,2275,2277,2279,2281,2283,2285,2287,2289,2291,2293,2295,2297,2299,2301,2303,2305,2307,2309,2311,2313,2315,2317,2319,2321,2323,2325,2327,2329,2331,2333,2335,2337,2339,2341,2343,2345,2403,2405,2407,2409,2411,2413,2415,2417,2419,2421,2423,2425,2427,2429,2431,2433,2435,2437,2439,2441,2443,2445,2447,2449,2451,2453,2455,2457,2459,2461,2463";
                const string leftYellowStr =
                    "103,105,107,109,111,113,115,117,119,121,123,205,207,209,211,213,215,217,219,221,223,225,227,229,231,233,235,237,332,334,336,338,340,342,344,346,348,350,352,354,356,358,360,362,364,366,368,466,468,470,472,474,476,478,480,482,484,486,488,490,492,494,496,498,500,502,504,605,607,609,611,613,615,617,619,621,623,625,627,629,631,633,635,637,639,641,643,645,755,757,759,761,763,765,767,769,771,773,775,777,779,781,783,785,787,789,791,793,795,905,907,909,911,913,915,917,919,921,923,925,927,929,931,933,935,937,939,941,943,945,947,949,1053,1055,1057,1059,1061,1063,1065,1067,1069,1071,1073,1075,1077,1079,1081,1083,1085,1087,1089,1091,1093,1095,1097,1202,1204,1206,1208,1210,1212,1214,1216,1218,1220,1222,1224,1226,1228,1230,1232,1234,1236,1238,1240,1242,1244,1246,1248,1250,1252,1254,1256,1258,1374,1376,1378,1380,1382,1384,1386,1388,1390,1392,1394,1396,1398,1400,1402,1404,1406,1408,1410,1412,1414,1416,1418,1420,1422,1424,1426,1428,1430,1432,1434,1555,1557,1559,1561,1563,1565,1567,1569,1571,1573,1575,1577,1579,1581,1583,1585,1587,1589,1591,1593,1595,1597,1599,1601,1603,1605,1607,1609,1611,1613,1615,1617,1739,1741,1743,1745,1747,1749,1751,1753,1755,1757,1759,1761,1763,1765,1767,1769,1771,1773,1775,1777,1779,1781,1783,1785,1787,1789,1791,1793,1795,1797,1799,1920,1922,1924,1926,1928,1930,1932,1934,1936,1938,1940,1942,1944,1946,1948,1950,1952,1954,1956,1958,1960,1962,1964,1966,1968,1970,1972,1974,1976,1978,1980,1982,2104,2106,2108,2110,2112,2114,2116,2118,2120,2122,2124,2126,2128,2130,2132,2134,2136,2138,2140,2142,2144,2146,2148,2150,2152,2154,2156,2158,2160,2162,2164,2285,2287,2289,2291,2293,2295,2297,2299,2301,2303,2305,2307,2309,2311,2313,2315,2317,2319,2321,2323,2325,2327,2329,2331,2333,2335,2337,2339,2341,2343,2345";
                const string rightYellowStr =
                    "181,183,185,187,189,191,193,195,197,199,201,296,298,300,302,304,306,308,310,312,314,316,318,320,322,324,326,328,426,428,430,432,434,436,438,440,442,444,446,448,450,452,454,456,458,460,462,563,565,567,569,571,573,575,577,579,581,583,585,587,589,591,593,595,597,599,601,703,705,707,709,711,713,715,717,719,721,723,725,727,729,731,733,735,737,739,741,743,854,856,858,860,862,864,866,868,870,872,874,876,878,880,882,884,886,888,890,892,894,1007,1009,1011,1013,1015,1017,1019,1021,1023,1025,1027,1029,1031,1033,1035,1037,1039,1041,1043,1045,1047,1049,1051,1156,1158,1160,1162,1164,1166,1168,1170,1172,1174,1176,1178,1180,1182,1184,1186,1188,1190,1192,1194,1196,1198,1200,1316,1318,1320,1322,1324,1326,1328,1330,1332,1334,1336,1338,1340,1342,1344,1346,1348,1350,1352,1354,1356,1358,1360,1362,1364,1366,1368,1370,1372,1493,1495,1497,1499,1501,1503,1505,1507,1509,1511,1513,1515,1517,1519,1521,1523,1525,1527,1529,1531,1533,1535,1537,1539,1541,1543,1545,1547,1549,1551,1553,1675,1677,1679,1681,1683,1685,1687,1689,1691,1693,1695,1697,1699,1701,1703,1705,1707,1709,1711,1713,1715,1717,1719,1721,1723,1725,1727,1729,1731,1733,1735,1737,1858,1860,1862,1864,1866,1868,1870,1872,1874,1876,1878,1880,1882,1884,1886,1888,1890,1892,1894,1896,1898,1900,1902,1904,1906,1908,1910,1912,1914,1916,1918,2040,2042,2044,2046,2048,2050,2052,2054,2056,2058,2060,2062,2064,2066,2068,2070,2072,2074,2076,2078,2080,2082,2084,2086,2088,2090,2092,2094,2096,2098,2100,2102,2223,2225,2227,2229,2231,2233,2235,2237,2239,2241,2243,2245,2247,2249,2251,2253,2255,2257,2259,2261,2263,2265,2267,2269,2271,2273,2275,2277,2279,2281,2283,2403,2405,2407,2409,2411,2413,2415,2417,2419,2421,2423,2425,2427,2429,2431,2433,2435,2437,2439,2441,2443,2445,2447,2449,2451,2453,2455,2457,2459,2461,2463";

                _isSleep = true;
                _ledDataPackages.Clear();
                _redLedList.Clear();
                _yellowLedList.Clear();
                _leftYellowLedList.Clear();
                _rightYellowLedList.Clear();

                foreach (var t in redStr.Split(','))
                {
                    int index;
                    if (!int.TryParse(t, out index))
                        continue;
                    index = index - 1;
                    if (!_redLedList.Contains(index))
                        _redLedList.Add(index);
                }

                foreach (var t in yellowStr.Split(','))
                {
                    int index;
                    if (!int.TryParse(t, out index))
                        continue;
                    index = index - 1;

                    if (!_yellowLedList.Contains(index))
                        _yellowLedList.Add(index);
                }

                foreach (var t in leftYellowStr.Split(','))
                {
                    int index;
                    if (!int.TryParse(t, out index))
                        continue;
                    index = index - 1;

                    if (!_leftYellowLedList.Contains(index))
                        _leftYellowLedList.Add(index);
                }

                foreach (var t in rightYellowStr.Split(','))
                {
                    int index;
                    if (!int.TryParse(t, out index))
                        continue;
                    index = index - 1;

                    if (!_rightYellowLedList.Contains(index))
                        _rightYellowLedList.Add(index);
                }

                _redLedList.Sort();
                _yellowLedList.Sort();
                _leftYellowLedList.Sort();
                _rightYellowLedList.Sort();

                var max = FindMax(_redLedList, _yellowLedList);

                var baseIndex = 0;
                for (uint i = 0x401; i < 0x4FF; i++)
                {
                    var groudIndex = max / 64;
                    if (baseIndex <= groudIndex)
                    {
                        _ledDataPackages.Add(
                            new CanBus.CanDataPackage(
                                i,
                                CanBus.CanProtocol.CanFd,
                                CanBus.CanType.Standard,
                                CanBus.CanFormat.Data,
                                new byte[64]));
                    }
                    baseIndex++;
                }
            }
        }

        [Description("切换成R_Left_A+C")]
        public void ChangeToRclaLeft()
        {
            lock (_ledDataPackages)
            {
                _canFdResponseIdList.Clear();
                for (uint i = 0x691; i <= 0x69F; i++)
                    _canFdResponseIdList.Add(i);

                _requestCanId = 0x784;
                _responseCanId = 0x794;

                const string redStr =
                    "1,3,5,7,9,11,13,15,17,19,21,23,25,27,29,31,32,34,36,38,40,42,44,46,48,50,52,54,56,58,60,62,64,66,68,70,72,74,76,78,80,82,84,86,88,90,92,94,96,98,100,102,104,106,108,110,112,114,116,118,120,122,124,126,128,130,132,134,136,138,140,142,144,146,148,150,152,154,156,158,160,162,164,166,168,170,172,174,176,178,180,182,184,186,188,190,192,194,196,198,200,202,204,206,208,210,212,214,216,218,220,222,224,226,228,230,232,234,236,238,240,242,244,246,248,250,252,254,256,258,260,262,264,266,268,270,272,274,276,278,280,282,284,286,288,290,292,294,296,298";
                const string yellowStr =
                    "33,35,37,39,41,43,45,47,49,51,53,55,57,59,61,63,65,67,69,71,73,75,77,79,81,83,85,87,89,91,93,95,97,99,101,103,105,107,109,111,113,115,117,119,121,123,125,127,129,131,133,135,137,139,141,143,145,147,149,151,153,155,157,159,161,163,165,167,169,171,173,175,177,179,181,183,185,187,189,191,193,195,197,199,201,203,205,207,209,211,213,215,217,219,221,223,225,227,229,231,233,235,237,239,241,243,245,247,249,251,253,255,257,259,261,263,265,267,269,271,273,275,277,279,281,283,285,287,289,291,293,295,297,299";

                _isSleep = true;
                _ledDataPackages.Clear();
                _redLedList.Clear();
                _yellowLedList.Clear();

                foreach (var t in redStr.Split(','))
                {
                    int index;
                    if (!int.TryParse(t, out index))
                        continue;
                    index = index - 1;
                    if (!_redLedList.Contains(index))
                        _redLedList.Add(index);
                }

                foreach (var t in yellowStr.Split(','))
                {
                    int index;
                    if (!int.TryParse(t, out index))
                        continue;
                    index = index - 1;

                    if (!_yellowLedList.Contains(index))
                        _yellowLedList.Add(index);
                }

                _redLedList.Sort();
                _yellowLedList.Sort();

                for (uint i = 0x301; i < 0x30F; i++)
                    _ledDataPackages.Add(
                        new CanBus.CanDataPackage(
                            i,
                            CanBus.CanProtocol.CanFd,
                            CanBus.CanType.Standard,
                            CanBus.CanFormat.Data,
                            new byte[64]));
            }
        }

        [Description("切换成R_Right_A+C")]
        public void ChangeToRclaRight()
        {
            lock (_ledDataPackages)
            {
                _canFdResponseIdList.Clear();
                for (uint i = 0x6B1; i <= 0x6BF; i++)
                    _canFdResponseIdList.Add(i);

                _requestCanId = 0x785;
                _responseCanId = 0x795;

                const string redStr =
                    "1,3,5,7,9,11,13,15,17,19,21,23,25,27,29,31,32,34,36,38,40,42,44,46,48,50,52,54,56,58,60,62,64,66,68,70,72,74,76,78,80,82,84,86,88,90,92,94,96,98,100,102,104,106,108,110,112,114,116,118,120,122,124,126,128,130,132,134,136,138,140,142,144,146,148,150,152,154,156,158,160,162,164,166,168,170,172,174,176,178,180,182,184,186,188,190,192,194,196,198,200,202,204,206,208,210,212,214,216,218,220,222,224,226,228,230,232,234,236,238,240,242,244,246,248,250,252,254,256,258,260,262,264,266,268,270,272,274,276,278,280,282,284,286,288,290,292,294,296,298";
                const string yellowStr =
                    "33,35,37,39,41,43,45,47,49,51,53,55,57,59,61,63,65,67,69,71,73,75,77,79,81,83,85,87,89,91,93,95,97,99,101,103,105,107,109,111,113,115,117,119,121,123,125,127,129,131,133,135,137,139,141,143,145,147,149,151,153,155,157,159,161,163,165,167,169,171,173,175,177,179,181,183,185,187,189,191,193,195,197,199,201,203,205,207,209,211,213,215,217,219,221,223,225,227,229,231,233,235,237,239,241,243,245,247,249,251,253,255,257,259,261,263,265,267,269,271,273,275,277,279,281,283,285,287,289,291,293,295,297,299";

                _isSleep = true;
                _ledDataPackages.Clear();
                _redLedList.Clear();
                _yellowLedList.Clear();

                foreach (var t in redStr.Split(','))
                {
                    int index;
                    if (!int.TryParse(t, out index))
                        continue;
                    index = index - 1;
                    if (!_redLedList.Contains(index))
                        _redLedList.Add(index);
                }

                foreach (var t in yellowStr.Split(','))
                {
                    int index;
                    if (!int.TryParse(t, out index))
                        continue;
                    index = index - 1;

                    if (!_yellowLedList.Contains(index))
                        _yellowLedList.Add(index);
                }

                _redLedList.Sort();
                _yellowLedList.Sort();

                for (uint i = 0x501; i < 0x50F; i++)
                    _ledDataPackages.Add(
                        new CanBus.CanDataPackage(
                            i,
                            CanBus.CanProtocol.CanFd,
                            CanBus.CanType.Standard,
                            CanBus.CanFormat.Data,
                            new byte[64]));
            }
        }

        private static int FindMax(
            IReadOnlyList<int> a, IReadOnlyCollection<int> b)
        {
            var list = new List<int>
            {
                a.Any() ? a[a.Count - 1] : 0,
                b.Any() ? a[b.Count - 1] : 0,
            };

            list.Sort();
            return list[1];
        }

        #endregion

        #region LED相关

        [Description("B灯左转向开")]
        public void RclbLeftTurnOn(string grayValue)
        {
            if (!_leftYellowLedList.Any())
                return;

            if (string.IsNullOrEmpty(grayValue))
                return;

            byte gray;
            if (!byte.TryParse(grayValue, out gray))
                return;

            foreach (var t in _ledDataPackages)
                for (var j = 0; j < t.CanDataLen; j++)
                    t.CanData[j] = 0x00;

            lock (_ledDataPackages)
            {
                foreach (var r in _leftYellowLedList)
                {
                    var groudIndex = r / 64;
                    var ledIndex = r % 64;

                    if (_ledDataPackages.Count < groudIndex)
                        continue;

                    _ledDataPackages[groudIndex].CanData[ledIndex] = gray;
                }
            }

            _isSleep = false;
        }
        
        [Description("B灯右转向开")]
        public void RclbRightTurnOn(string grayValue)
        {
            if (!_rightYellowLedList.Any())
                return;

            if (string.IsNullOrEmpty(grayValue))
                return;

            byte gray;
            if (!byte.TryParse(grayValue, out gray))
                return;

            foreach (var t in _ledDataPackages)
                for (var j = 0; j < t.CanDataLen; j++)
                    t.CanData[j] = 0x00;

            lock (_ledDataPackages)
            {
                foreach (var r in _rightYellowLedList)
                {
                    var groudIndex = r / 64;
                    var ledIndex = r % 64;

                    if (_ledDataPackages.Count < groudIndex)
                        continue;

                    _ledDataPackages[groudIndex].CanData[ledIndex] = gray;
                }
            }

            _isSleep = false;
        }

        [Description("点亮所有红光LED")]
        public void RedLedOn(string grayValue)
        {
            if (string.IsNullOrEmpty(grayValue))
                return;

            byte gray;
            if (!byte.TryParse(grayValue, out gray))
                return;

            foreach (var t in _ledDataPackages)
                for (var j = 0; j < t.CanDataLen; j++)
                    t.CanData[j] = 0x00;

            lock (_ledDataPackages)
            {
                foreach (var r in _redLedList)
                {
                    var groudIndex = r / 64;
                    var ledIndex = r % 64;

                    if (_ledDataPackages.Count < groudIndex)
                        continue;

                    _ledDataPackages[groudIndex].CanData[ledIndex] = gray;
                }
            }

            _isSleep = false;
        }

        [Description("点亮所有单数红光LED")]
        public void RedLedOddOn(string grayValue)
        {
            if (string.IsNullOrEmpty(grayValue))
                return;

            byte gray;
            if (!byte.TryParse(grayValue, out gray))
                return;

            lock (_ledDataPackages)
            {
                foreach (var t in _ledDataPackages)
                    for (var j = 0; j < t.CanDataLen; j++)
                        t.CanData[j] = 0x00;

                for (var i = 0; i < _redLedList.Count; i++)
                {
                    if (i % 2 != 0)
                        continue;
                    var r = _redLedList[i];

                    var groudIndex = r / 64;
                    var ledIndex = r % 64;

                    if (_ledDataPackages.Count < groudIndex)
                        continue;

                    _ledDataPackages[groudIndex].CanData[ledIndex] = gray;
                }
            }

            _isSleep = false;
        }

        [Description("点亮所有双数红光LED")]
        public void RedLedEvenOn(string grayValue)
        {
            if (string.IsNullOrEmpty(grayValue))
                return;

            byte gray;
            if (!byte.TryParse(grayValue, out gray))
                return;

            lock (_ledDataPackages)
            {
                foreach (var t in _ledDataPackages)
                    for (var j = 0; j < t.CanDataLen; j++)
                        t.CanData[j] = 0x00;

                for (var i = 0; i < _redLedList.Count; i++)
                {
                    if (i % 2 == 0)
                        continue;
                    var r = _redLedList[i];

                    var groudIndex = r / 64;
                    var ledIndex = r % 64;

                    if (_ledDataPackages.Count < groudIndex)
                        continue;

                    _ledDataPackages[groudIndex].CanData[ledIndex] = gray;
                }
            }

            _isSleep = false;
        }

        [Description("点亮所有黄光LED")]
        public void YelowLedOn(string grayValue)
        {
            if (string.IsNullOrEmpty(grayValue))
                return;

            byte gray;
            if (!byte.TryParse(grayValue, out gray))
                return;

            lock (_ledDataPackages)
            {
                foreach (var t in _ledDataPackages)
                    for (var j = 0; j < t.CanDataLen; j++)
                        t.CanData[j] = 0x00;

                foreach (var r in _yellowLedList)
                {
                    var groudIndex = r / 64;
                    var ledIndex = r % 64;

                    if (_ledDataPackages.Count < groudIndex)
                        continue;

                    _ledDataPackages[groudIndex].CanData[ledIndex] = gray;
                }
            }

            _isSleep = false;
        }

        [Description("点亮所有单数黄光LED")]
        public void YellowLedOddOn(string grayValue)
        {
            if (string.IsNullOrEmpty(grayValue))
                return;

            byte gray;
            if (!byte.TryParse(grayValue, out gray))
                return;

            lock (_ledDataPackages)
            {
                foreach (var t in _ledDataPackages)
                    for (var j = 0; j < t.CanDataLen; j++)
                        t.CanData[j] = 0x00;

                for (var i = 0; i < _yellowLedList.Count; i++)
                {
                    if (i % 2 != 0)
                        continue;
                    var r = _yellowLedList[i];

                    var groudIndex = r / 64;
                    var ledIndex = r % 64;

                    if (_ledDataPackages.Count < groudIndex)
                        continue;

                    _ledDataPackages[groudIndex].CanData[ledIndex] = gray;
                }
            }

            _isSleep = false;
        }

        [Description("点亮所有双数黄光LED")]
        public void YellowLedEvenOn(string grayValue)
        {
            if (string.IsNullOrEmpty(grayValue))
                return;

            byte gray;
            if (!byte.TryParse(grayValue, out gray))
                return;

            lock (_ledDataPackages)
            {
                foreach (var t in _ledDataPackages)
                    for (var j = 0; j < t.CanDataLen; j++)
                        t.CanData[j] = 0x00;

                for (var i = 0; i < _yellowLedList.Count; i++)
                {
                    if (i % 2 == 0)
                        continue;
                    var r = _yellowLedList[i];

                    var groudIndex = r / 64;
                    var ledIndex = r % 64;

                    if (_ledDataPackages.Count < groudIndex)
                        continue;

                    _ledDataPackages[groudIndex].CanData[ledIndex] = gray;
                }
            }

            _isSleep = false;
        }

        [Description("关闭所有LED")]
        public void AllLedOff()
        {
            _isSleep = true;

            lock (_ledDataPackages)
            {
                foreach (var p in _ledDataPackages)
                {
                    for (var i = 0; i < p.CanDataLen; i++)
                        p.CanData[i] = 0x00;
                }
            }
        }
        #endregion

        #region 信息读取相关
        [Description("读软件版本号")]
        public void ReadSoftwareVersion()
        {
            SoftwareVersion = string.Empty;
            if (CanFd == null)
                return;

            byte[] echo;
            if (CanFd.CanBusWithUds.TryReadData(_requestCanId, _responseCanId,
                CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xf1, 0x95, out echo, 0x00))
                SoftwareVersion = Encoding.ASCII.GetString(echo);
            else
            {
                Thread.Sleep(500);
                if (CanFd.CanBusWithUds.TryReadData(_requestCanId, _responseCanId,
                    CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xf1, 0x95, out echo, 0x00))
                    SoftwareVersion = Encoding.ASCII.GetString(echo);
            }
        }

        [Description("读硬件版本号")]
        public void ReadHardwareVersion()
        {
            HardwareVersion = string.Empty;
            if (CanFd == null)
                return;

            byte[] echo;
            if (CanFd.CanBusWithUds.TryReadData(_requestCanId, _responseCanId,
                CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xf1, 0x93, out echo, 0x00))
                HardwareVersion = Encoding.ASCII.GetString(echo);
            else
            {
                Thread.Sleep(500);
                if (CanFd.CanBusWithUds.TryReadData(_requestCanId, _responseCanId,
                CanBus.CanType.Standard, CanBus.CanProtocol.Can, 0xf1, 0x93, out echo, 0x00))
                    HardwareVersion = Encoding.ASCII.GetString(echo);
            }
        }

        /// <summary>
        /// 读取故障信息
        /// </summary>
        [Description("读取故障信息")]
        public void FaultDetect()
        {
            FaultRead = string.Empty;
            if (CanFd == null)
                return;

            foreach (var t in _canFdResponseIdList)
                CanFd.AddDoNotFilterCanId(t);

            CanFd.CanRecvDataPackages.Clear();
            Thread.Sleep(2000);

            if (CanFd.CanRecvDataPackages.Any())
            {
                var temp = CanFd.CanRecvDataPackages.ToArray();

                try
                {
                    foreach (var find in _canFdResponseIdList.Select(item => temp.ToList().Find(f => f.CanId == item)))
                    {
                        if (find == null)
                        {
                            FaultRead = string.Empty;
                            break;
                        }
                        var datas = new List<byte>();
                        datas.AddRange(find.CanData);
                        FaultRead += ValueHelper.GetHextStr(datas.ToArray());
                        FaultRead += " ";
                    }

                    FaultRead = FaultRead.TrimEnd(' ');
                }
                catch (Exception)
                {
                    FaultRead = string.Empty;
                }
            }

            foreach (var t in _canFdResponseIdList)
                CanFd.RemoveDoNotFilterCanId(t);
        }
        #endregion
    }
}
