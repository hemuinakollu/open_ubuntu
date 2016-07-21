// Copyright © 2002 - 2005 DataDirect Technologies Corp. All Rights Reserved.
using System;
using System.Collections.Generic;
using System.Text;

namespace DDInt.Utility
{
	public abstract class UtilTransliterator
	{
		internal const string footprint = "$Revision:   1.17.1.0  $";
        private static object m_syncObject = new object();

		//internal static Encoding GetEncoding(string name) {

		//	return GetEncoding(name, null, null);
		//}

		//internal static Encoding GetEncoding(int codepage) {

		//	return GetEncoding(codepage, null, null);
		//}

		private static Dictionary<string, bool> s_hasEncoding;
        private static Dictionary<int, bool> ss_hasEncoding;
        private static bool HasEncoding(string name) {
            lock (m_syncObject){
                if (s_hasEncoding == null){
                    s_hasEncoding = new Dictionary<string,bool>();
                }
                if (s_hasEncoding.ContainsKey(name)){
                    return (bool)s_hasEncoding[name];
                }
			    try {

				    // We were using the Encoding.GetEncodings() method to search for a
				    // supported encoding but the name returned by the EncodingInfo.Name
				    // property was not always the IANA name.
				    Encoding encoding = Encoding.GetEncoding(name);
				    s_hasEncoding.Add(name, true);

				    return true;
			    }
			    catch {
				    s_hasEncoding.Add(name, false);
				    return false;
			    }
            }
		}

		private static bool HasEncoding(int codepage) {
            lock (m_syncObject){
                if (s_hasEncoding == null){
                    ss_hasEncoding = new Dictionary<int, bool>();
                }

                if (ss_hasEncoding.ContainsKey(codepage)){
                    return (bool)ss_hasEncoding[codepage];
                }

                try{
                    Encoding encoding = Encoding.GetEncoding(codepage);
                    ss_hasEncoding.Add(codepage, true);

                    return true;
                }
                catch{
                    ss_hasEncoding.Add(codepage, false);
                    return false;
                }
            }
		}

		/// <summary>
		/// Gets codepage by name.
		/// </summary>
		//internal static Encoding GetEncoding(string name, EncoderFallback encoderFallback, DecoderFallback decoderFallback) {
			
		//	string ianaName = AliasToIANAName(name);
		//	string icuMapName = AliasToICUMapName(name);

		//	try {

		//		if (HasEncoding(name)) {

		//			// try .NET Framework encoders
		//			if (encoderFallback == null || decoderFallback == null) {

		//				// Use non-fallback method if null was specified
		//				return Encoding.GetEncoding(ianaName != null ? ianaName : name);
		//			}

		//			return Encoding.GetEncoding(ianaName != null ? ianaName : name, encoderFallback, decoderFallback);
		//		}
		//	}
		//	catch { }

		//	try {

		//		if (UtilTransliteratorUsingTable.HasEncoding(ianaName != null ? ianaName : name)) {

		//			return UtilTransliteratorUsingTable.GetEncoding(ianaName != null ? ianaName : name, encoderFallback, decoderFallback);
		//		}
		//	}
		//	catch { }

		//	return UtilTransliteratorUsingICUTable.GetEncoding(icuMapName != null ? icuMapName : name, encoderFallback, decoderFallback);
		//}

		/// <summary>
		/// Gets codepage by windows codepage number.
		/// </summary>
		//internal static Encoding GetEncoding(int codePage, EncoderFallback encoderFallback, DecoderFallback decoderFallback) {

		//	try {

		//		if (HasEncoding(codePage)) {

		//			// try .NET Framework encoders
		//			if (encoderFallback == null || decoderFallback == null) {

		//				// Use non-fallback method if null was specified
		//				return Encoding.GetEncoding(codePage);
		//			}

		//			return Encoding.GetEncoding(codePage, encoderFallback, decoderFallback);
		//		}
		//	} 
		//	catch { }

		//	try {

		//		if (UtilTransliteratorUsingTable.HasEncoding(codePage)) {

		//			return UtilTransliteratorUsingTable.GetEncoding(codePage, encoderFallback, decoderFallback);
		//		}
		//	} 
		//	catch { }
			
		//	return UtilTransliteratorUsingICUTable.GetEncoding(codePage, encoderFallback, decoderFallback);
		//}

		/// <summary>
		/// Maps a character set alias to an IANA character set name.
		/// This mapping supports only the code pages implemented by DDTek.Util.
		/// Based on http://www.iana.org/assignments/character-sets with some additions
		/// </summary>
		/// <returns>Name or null if not found</returns>
		internal static string AliasToIANAName( string alias ) {
			return (string)alias2Name[alias.ToUpper()];
		}

		/// <summary>
		/// Maps a character set alias to an ICU character map name.
		/// This mapping supports only the code pages implemented by DDTek.Util.
		/// Based on http://oss.software.ibm.com/cvs/icu/charset/data/ucm/ 
		/// </summary>
		/// <returns>Name or null if not found</returns>
		internal static string AliasToICUMapName( string alias ) {
			return (string)alias2icuName[alias.ToUpper()];
		}

		/// <summary>
		/// Maps a Windows codepage nr to an IANA character set name.
		/// This mapping supports only the code pages implemented by DDTek.Util.
		/// </summary>
		/// <returns>Name or null if not found</returns>
		internal static string CodepageToIANAName( int codePage ) {
			return (string)cp2Name[codePage];
		}

		/// <summary>
		/// Maps a codepage nr to an ICU character map name.
		/// This mapping supports only the code pages implemented by DDTek.Util.
		/// </summary>
		/// <returns>Name or null if not found</returns>
		internal static string CodepageToICUMapName( int codePage ) {
			return (string)cp2icuName[codePage];
		}

		/// <summary>
		/// Maps an IANA character set name to a codepage nr.
		/// This mapping supports only the code pages implemented by DDTek.Util.
		/// </summary>
		/// <returns>Codepage or 0 if not found</returns>
		internal static int IANANameToCodepage( string name ) {
			object o = name2Cp[name.ToUpper()];
			return o != null ? (int)o : 0;
		}

		/// <summary>
		/// Maps an ICU character map name to a codepage nr.
		/// This mapping supports only the code pages implemented by DDTek.Util.
		/// </summary>
		/// <returns>Codepage or 0 if not found</returns>
		internal static int ICUMapNameToCodepage( string name ) {
			object o = icuName2Cp[name.ToUpper()];
			return o != null ? (int)o : 0;
		}

		private class TableEntry
		{ 
			internal TableEntry(int i, string s) {codePage = i; name = s;}
			internal int codePage; 
			internal string name; 
		}

		//-------------------------
		// IANA mappings
		//-------------------------
		# region IANA mappings
		private static TableEntry[] table1 = new TableEntry[] {
			new TableEntry(37,    "IBM037"),
			new TableEntry(273,   "IBM273"),
			new TableEntry(277,   "IBM277"),
			new TableEntry(278,   "IBM278"),
			new TableEntry(280,   "IBM280"),
			new TableEntry(284,   "IBM284"),
			new TableEntry(285,   "IBM285"),
			new TableEntry(290,   "IBM290"),
			new TableEntry(297,   "IBM297"),
			new TableEntry(367,   "ASCII"),
			new TableEntry(500,   "IBM500"),
			new TableEntry(850,   "IBM850"),
			new TableEntry(895,   "IBM895"),    // = Japanese 7 bit
			new TableEntry(897,   "IBM897"),    // = Japanese PC#1
			new TableEntry(954,   "EUC-JP"),    // = EUC-JP
			new TableEntry(1086,  "IBM01086"),  // = Japanese PC#1 variant
			new TableEntry(1140,  "IBM01140"),  // = 37 + EURO
			new TableEntry(1141,  "IBM01141"),  // = 273 + EURO
			new TableEntry(1142,  "IBM01142"),  // = 277 + EURO
			new TableEntry(1143,  "IBM01143"),  // = 278 + EURO
			new TableEntry(1144,  "IBM01144"),  // = 280 + EURO
			new TableEntry(1145,  "IBM01145"),  // = 284 + EURO
			new TableEntry(1146,  "IBM01146"),  // = 285 + EURO
			new TableEntry(1147,  "IBM01147"),  // = 297 + EURO
			new TableEntry(1148,  "IBM01148"),  // = 500 + EURO
			new TableEntry(1149,  "IBM01149"),  // = 871 + EURO
			new TableEntry(1051,  "HP-ROMAN8"), // ???
			new TableEntry(1200,  "UTF-16"),
			new TableEntry(1208,  "UTF-8"),
			new TableEntry(1252,  "WINDOWS-1252"),
			new TableEntry(13488, "UCS-2"),
			new TableEntry(28591, "ISO_8859-1") // ???
		};
		#endregion

		//-------------------------
		// ICU mappings
		//-------------------------
		# region ICU mappings
		private static TableEntry[] table2 = new TableEntry[] {
			new TableEntry(300,	  "IBM-300_P110-1997"),
			new TableEntry(301,	  "IBM-301_P110-1997"),
			new TableEntry(834,	  "IBM-834_P100-1995"),
			new TableEntry(835,	  "IBM-835_P100-1995"),
			new TableEntry(837,	  "IBM-837_P100-1995"),
			new TableEntry(927,	  "IBM-927_P100-1995"),
			new TableEntry(930,	  "IBM-930_P120-1999"),
			new TableEntry(932,	  "IBM-932_P120-1999"),
			new TableEntry(933,	  "IBM-933_P110-1995"),
			new TableEntry(935,	  "IBM-935_P110-1999"),
			new TableEntry(937,	  "IBM-937_P110-1999"),
			new TableEntry(939,	  "IBM-939_P120-1999"),
			new TableEntry(941,	  "IBM-941_P120-1996"),
			new TableEntry(942,	  "IBM-942_P120-1999"),
			new TableEntry(943,	  "IBM-943_P130-1999"),
			new TableEntry(947,	  "IBM-947_P100-1995"),
			new TableEntry(948,	  "IBM-948_P110-1999"),
			new TableEntry(949,	  "IBM-949_P110-1999"),
			new TableEntry(950,	  "IBM-950_P110-1999"),
			new TableEntry(951,	  "IBM-951_P100-1995"),
			new TableEntry(970,	  "IBM-970_P110-1995"),
			new TableEntry(971,	  "IBM-971_P100-1995"),
			new TableEntry(1351,  "IBM-1351_P110-1997"),
			new TableEntry(1362,  "IBM-1362_P110-1999"),
			new TableEntry(1363,  "IBM-1363_P110-1997"),
			new TableEntry(1364,  "IBM-1364_P110-1997"),
			new TableEntry(1380,  "IBM-1380_P100-1995"),
			new TableEntry(1381,  "IBM-1381_P110-1999"),
			new TableEntry(1382,  "IBM-1382_P100-1995"),
			new TableEntry(1383,  "IBM-1383_P110-1999"),
			new TableEntry(1385,  "IBM-1385_P100-1997"),
			new TableEntry(1386,  "IBM-1386_P100-2001"),
			new TableEntry(1388,  "IBM-1388_P110-2000"),
			new TableEntry(1399,  "IBM-1399_P110-2003"),
			new TableEntry(4930,  "IBM-4930_P100-1997"),
			new TableEntry(4933,  "IBM-4933_P100-2002"),
			new TableEntry(5026,  "IBM-5026_P120-1999"),
			new TableEntry(5035,  "IBM-5035_P120-1999"),
			new TableEntry(5039,  "IBM-5039_P110-1996"),
			new TableEntry(16684, "IBM-16684_P110-2003")
		};
		#endregion

		static UtilTransliterator()
		{
			foreach( TableEntry entry in table1	) {
				cp2Name.Add(entry.codePage, entry.name);
				name2Cp.Add(entry.name, entry.codePage);
			}

			foreach( TableEntry entry in table2	) {
				cp2icuName.Add(entry.codePage, entry.name);
				icuName2Cp.Add(entry.name, entry.codePage);
			}

			//---------------------------
			// Aliases for IANA codepages
			//---------------------------
			# region IANAname aliases
			// Aliases for ASCII
			alias2Name.Add("ASCII",           "ASCII");
			alias2Name.Add("CP367",           "ASCII");
			alias2Name.Add("CSIBM367",        "ASCII");
			alias2Name.Add("CSASCII",         "ASCII");
			alias2Name.Add("IBM367",          "ASCII");
			alias2Name.Add("US",              "ASCII");
			alias2Name.Add("US-ASCII",        "ASCII");
			alias2Name.Add("ISO646-US",       "ASCII");
			alias2Name.Add("ISO_646.IRV:1991","ASCII");
			alias2Name.Add("ANSI_X3.4-1986",  "ASCII");
			alias2Name.Add("ISO-IR-6",        "ASCII");
			alias2Name.Add("ANSI_X3.4-1968",  "ASCII");

			// Aliases for HP-ROMAN8
			alias2Name.Add("HP-ROMAN8",       "HP-ROMAN8");
			alias2Name.Add("ROMAN8",          "HP-ROMAN8");
			alias2Name.Add("R8",              "HP-ROMAN8");
			alias2Name.Add("CSHPROMAN8",      "HP-ROMAN8");

			// Aliases for IBM01140
			alias2Name.Add("IBM01140",        "IBM01140");
			alias2Name.Add("CP01140",         "IBM01140");
			alias2Name.Add("CCSID01140",      "IBM01140");
			alias2Name.Add("EBCDIC-US-37+EURO", "IBM01140");

			// Aliases for IBM01141
			alias2Name.Add("IBM01141",        "IBM01141");
			alias2Name.Add("CP01141",         "IBM01141");
			alias2Name.Add("CCSID01141",      "IBM01141");
			alias2Name.Add("EBCDIC-DE-273+EURO", "IBM01141");

			// Aliases for IBM01142
			alias2Name.Add("IBM01142",        "IBM01142");
			alias2Name.Add("CP01142",         "IBM01142");
			alias2Name.Add("CCSID01142",      "IBM01142");
			alias2Name.Add("EBCDIC-DK-277+EURO", "IBM01142");
			alias2Name.Add("EBCDIC-NO-277+EURO", "IBM01142");

			// Aliases for IBM01143
			alias2Name.Add("IBM01143",        "IBM01143");
			alias2Name.Add("CP01143",         "IBM01143");
			alias2Name.Add("CCSID01143",      "IBM01143");
			alias2Name.Add("EBCDIC-FI-278+EURO", "IBM01143");
			alias2Name.Add("EBCDIC-SE-278+EURO", "IBM01143");

			// Aliases for IBM01144
			alias2Name.Add("IBM01144",        "IBM01144");
			alias2Name.Add("CP01144",         "IBM01144");
			alias2Name.Add("CCSID01144",      "IBM01144");
			alias2Name.Add("EBCDIC-IT-280+EURO", "IBM01144");

			// Aliases for IBM01145
			alias2Name.Add("IBM01145",        "IBM01145");
			alias2Name.Add("CP01145",         "IBM01145");
			alias2Name.Add("CCSID01145",      "IBM01145");
			alias2Name.Add("EBCDIC-ES-284+EURO", "IBM01145");

			// Aliases for IBM01146
			alias2Name.Add("IBM01146",        "IBM01146");
			alias2Name.Add("CP01146",         "IBM01146");
			alias2Name.Add("CCSID01146",      "IBM01146");
			alias2Name.Add("EBCDIC-GB-285+EURO", "IBM01146");

			// Aliases for IBM01147
			alias2Name.Add("IBM01147",        "IBM01147");
			alias2Name.Add("CP01147",         "IBM01147");
			alias2Name.Add("CCSID01147",      "IBM01147");
			alias2Name.Add("EBCDIC-FR-297+EURO", "IBM01147");

			// Aliases for IBM01148
			alias2Name.Add("IBM01148",        "IBM01148");
			alias2Name.Add("CP01148",         "IBM01148");
			alias2Name.Add("CCSID01148",      "IBM01148");
			alias2Name.Add("EBCDIC-INTERNATIONAL-500+EURO", "IBM01148");

			// Aliases for IBM01149
			alias2Name.Add("IBM01149",        "IBM01149");
			alias2Name.Add("CP01149",         "IBM01149");
			alias2Name.Add("CCSID01149",      "IBM01149");
			alias2Name.Add("EBCDIC-IS-871+EURO", "IBM01149");

			// Aliases for IBM037
			alias2Name.Add("IBM037",          "IBM037");
			alias2Name.Add("CP037",           "IBM037");
			alias2Name.Add("CSIBM037",        "IBM037");
			alias2Name.Add("EBCDIC-CP-US",    "IBM037");
			alias2Name.Add("EBCDIC-CP-CA",    "IBM037");
			alias2Name.Add("EBCDIC-CP-NL",    "IBM037");
			alias2Name.Add("EBCDIC-CP-WT",    "IBM037");

			// Aliases for IBM273
			alias2Name.Add("IBM273",          "IBM273");
			alias2Name.Add("CP273",           "IBM273");
			alias2Name.Add("CSIBM273",        "IBM273");

			// Aliases for IBM277
			alias2Name.Add("IBM277",          "IBM277");
			alias2Name.Add("CP277",           "IBM277");
			alias2Name.Add("CSIBM277",        "IBM277");
			alias2Name.Add("EBCDIC-CP-DK",    "IBM277");
			alias2Name.Add("EBCDIC-CP-NO",    "IBM277");

			// Aliases for IBM278
			alias2Name.Add("IBM278",          "IBM278");
			alias2Name.Add("CP278",           "IBM278");
			alias2Name.Add("CSIBM278",        "IBM278");
			alias2Name.Add("EBCDIC-CP-FI",    "IBM278");
			alias2Name.Add("EBCDIC-CP-SE",    "IBM278");

			// Aliases for IBM280
			alias2Name.Add("IBM280",          "IBM280");
			alias2Name.Add("CP280",           "IBM280");
			alias2Name.Add("CSIBM280",        "IBM280");
			alias2Name.Add("EBCDIC-CP-IT",    "IBM280");

			// Aliases for IBM284
			alias2Name.Add("IBM284",          "IBM284");
			alias2Name.Add("CP284",           "IBM284");
			alias2Name.Add("CSIBM284",        "IBM284");
			alias2Name.Add("EBCDIC-CP-ES",    "IBM284");

			// Aliases for IBM285
			alias2Name.Add("IBM285",          "IBM285");
			alias2Name.Add("CP285",           "IBM285");
			alias2Name.Add("CSIBM285",        "IBM285");
			alias2Name.Add("EBCDIC-CP-GB",    "IBM285");

			// Aliases for IBM290
			alias2Name.Add("IBM290",          "IBM290");
			alias2Name.Add("CP290",           "IBM290");
			alias2Name.Add("CSIBM290",        "IBM290");
			alias2Name.Add("EBCDIC-JP-kana",  "IBM290");

			// Aliases for IBM297
			alias2Name.Add("IBM297",          "IBM297");
			alias2Name.Add("CP297",           "IBM297");
			alias2Name.Add("CSIBM297",        "IBM297");
			alias2Name.Add("EBCDIC-CP-FR",    "IBM297");

			// Aliases for IBM500
			alias2Name.Add("IBM500",          "IBM500");
			alias2Name.Add("CP500",           "IBM500");
			alias2Name.Add("CSIBM500",        "IBM500");
			alias2Name.Add("EBCDIC-CP-CH",    "IBM500");
			alias2Name.Add("EBCDIC-CP-BE",    "IBM500");

			// Aliases for IBM850
			alias2Name.Add("IBM850",          "IBM850");
			alias2Name.Add("CP850",           "IBM850");

			// Aliases for IBM871
			alias2Name.Add("CSIBM871",        "IBM871");
			alias2Name.Add("CP871",           "IBM871");
			alias2Name.Add("EBCDIC-CP-IS",    "IBM871");

			// Aliases for ISO_8859-1
			alias2Name.Add("ISO_8859-1",      "ISO_8859-1");
			alias2Name.Add("ISO-IR-100",      "ISO_8859-1");
			alias2Name.Add("IBM819",          "ISO_8859-1");
			alias2Name.Add("CP819",           "ISO_8859-1");
			alias2Name.Add("CSISOLATIN1",     "ISO_8859-1");
			alias2Name.Add("LATIN1",          "ISO_8859-1");
			alias2Name.Add("L1",              "ISO_8859-1");

			// Aliases for WINDOWS-1252
			alias2Name.Add("WINDOWS-1252",    "WINDOWS-1252");
			alias2Name.Add("1252",            "WINDOWS-1252");

			// Aliases not from http://www.iana.org/assignments/character-sets
			alias2Name.Add("CP1252",		  "WINDOWS-1252");
			alias2Name.Add("UCS2",			  "UCS-2");
			alias2Name.Add("UTF7",			  "UTF-7");
			alias2Name.Add("UTF8",			  "UTF-8");
			alias2Name.Add("954",		      "EUC-JP");
			alias2Name.Add("1208",		      "UTF-8");
			alias2Name.Add("1200",			  "UTF-16");
			alias2Name.Add("13488",			  "UCS-2");
			alias2Name.Add("IBM875",		  "CP875");
			#endregion

			//-------------------------
			// Aliases for ICU mappings
			//-------------------------
			# region ICU map aliases
			//Japanese (Extended English) EBCDIC DBCS
			alias2icuName.Add("300",		  "IBM-300_P110-1997");
			alias2icuName.Add("CP300",		  "IBM-300_P110-1997");
			alias2icuName.Add("IBM300",		  "IBM-300_P110-1997");

			//Japanese (Extended) ASCII DBCS
			alias2icuName.Add("301",		  "IBM-301_P110-1997");
			alias2icuName.Add("CP301",		  "IBM-301_P110-1997");
			alias2icuName.Add("IBM301",		  "IBM-301_P110-1997");

			//Korean EBCDIC DBCS
			alias2icuName.Add("834",		  "IBM-834_P100-1995");
			alias2icuName.Add("CP834",		  "IBM-834_P100-1995");
			alias2icuName.Add("IBM834",		  "IBM-834_P100-1995");

			//Traditional Chinese EBCDIC DBCS
			alias2icuName.Add("835",		  "IBM-835_P100-1995");
			alias2icuName.Add("CP835",		  "IBM-835_P100-1995");
			alias2icuName.Add("IBM835",		  "IBM-835_P100-1995");

			//Simplified Chinese EBCDIC DBCS
			alias2icuName.Add("837",		  "IBM-837_P100-1995");
			alias2icuName.Add("CP837",		  "IBM-837_P100-1995");
			alias2icuName.Add("IBM837",		  "IBM-837_P100-1995");

			//Traditional Chinese ASCII DBCS
			alias2icuName.Add("927",		  "IBM-927_P100-1995");
			alias2icuName.Add("CP927",		  "IBM-927_P100-1995");
			alias2icuName.Add("IBM927",		  "IBM-927_P100-1995");

			// EBCDIC host mixed, Katakana-Kanji
			alias2icuName.Add("930",		"IBM-930_P120-1999");
			alias2icuName.Add("CP930",		"IBM-930_P120-1999");
			alias2icuName.Add("IBM930",		"IBM-930_P120-1999");

			//Japanese ASCII MBCS
			alias2icuName.Add("932",		  "IBM-932_P120-1999");
			alias2icuName.Add("CP932",		  "IBM-932_P120-1999");
			alias2icuName.Add("IBM932",		  "IBM-932_P120-1999");

			//Korean EBCDIC MBCS
			alias2icuName.Add("933",		  "IBM-933_P110-1995");
			alias2icuName.Add("CP933",		  "IBM-933_P110-1995");
			alias2icuName.Add("IBM933",		  "IBM-933_P110-1995");

			//Simplified Chinese EBCDIC MBCS
			alias2icuName.Add("935",		  "IBM-935_P110-1999");
			alias2icuName.Add("CP935",		  "IBM-935_P110-1999");
			alias2icuName.Add("IBM935",		  "IBM-935_P110-1999");

			//Traditional Chinese EBCDIC MBCS
			alias2icuName.Add("937",		  "IBM-937_P110-1999");
			alias2icuName.Add("CP937",		  "IBM-937_P110-1999");
			alias2icuName.Add("IBM937",		  "IBM-937_P110-1999");

			//Japanese (Extended English) EBCDIC MBCS
			alias2icuName.Add("939",		  "IBM-939_P120-1999");
			alias2icuName.Add("CP939",		  "IBM-939_P120-1999");
			alias2icuName.Add("IBM939",		  "IBM-939_P120-1999");

			//Japanese (Open environment) ASCII DBCS
			alias2icuName.Add("941",		  "IBM-941_P120-1996");
			alias2icuName.Add("CP941",		  "IBM-941_P120-1996");
			alias2icuName.Add("IBM941",		  "IBM-941_P120-1996");

			//Japanese (Extended) ASCII MBCS
			alias2icuName.Add("942",		  "IBM-942_P120-1999");
			alias2icuName.Add("CP942",		  "IBM-942_P120-1999");
			alias2icuName.Add("IBM942",		  "IBM-942_P120-1999");

			//Japanese (Open environment) ASCII MBCS
			alias2icuName.Add("943",		  "IBM-943_P130-1999");
			alias2icuName.Add("CP943",		  "IBM-943_P130-1999");
			alias2icuName.Add("IBM943",		  "IBM-943_P130-1999");

			//Traditional Chinese (IBM Big-5) ASCII DBCS
			alias2icuName.Add("947",		  "IBM-947_P100-1995");
			alias2icuName.Add("CP947",		  "IBM-947_P100-1995");
			alias2icuName.Add("IBM947",		  "IBM-947_P100-1995");

			//Traditional Chinese ASCII MBCS
			alias2icuName.Add("948",		  "IBM-948_P110-1999");
			alias2icuName.Add("CP948",		  "IBM-948_P110-1999");
			alias2icuName.Add("IBM948",		  "IBM-948_P110-1999");

			//Korean ASCII MBCS
			alias2icuName.Add("949",		  "IBM-949_P110-1999");
			alias2icuName.Add("CP949",		  "IBM-949_P110-1999");
			alias2icuName.Add("IBM949",		  "IBM-949_P110-1999");

			//Traditional Chinese (IBM Big-5) ASCII MBCS
			alias2icuName.Add("950",		  "IBM-950_P110-1999");
			alias2icuName.Add("CP950",		  "IBM-950_P110-1999");
			alias2icuName.Add("IBM950",		  "IBM-950_P110-1999");

			//Korean ASCII DBCS
			alias2icuName.Add("951",		  "IBM-951_P100-1995");
			alias2icuName.Add("CP951",		  "IBM-951_P100-1995");
			alias2icuName.Add("IBM951",		  "IBM-951_P100-1995");

			//Korean (EUC) ASCII MBCS
			alias2icuName.Add("970",		  "IBM-970_P110-1995");
			alias2icuName.Add("CP970",		  "IBM-970_P110-1995");
			alias2icuName.Add("IBM970",		  "IBM-970_P110-1995");

			//Korean (EUC) ASCII DBCS
			alias2icuName.Add("971",		  "IBM-971_P100-1995");
			alias2icuName.Add("CP971",		  "IBM-971_P100-1995");
			alias2icuName.Add("IBM971",		  "IBM-971_P100-1995");

			//Japanese (HP) ASCII DBCS
			alias2icuName.Add("1351",		  "IBM-1351_P110-1997");
			alias2icuName.Add("CP1351",		  "IBM-1351_P110-1997");
			alias2icuName.Add("IBM1351",	  "IBM-1351_P110-1997");

			//Korean ASCII DBCS
			alias2icuName.Add("1362",		  "IBM-1362_P110-1999");
			alias2icuName.Add("CP1362",		  "IBM-1362_P110-1999");
			alias2icuName.Add("IBM1362",	  "IBM-1362_P110-1999");

			//Korean ASCII MBCS
			alias2icuName.Add("1363",		  "IBM-1363_P110-1997");
			alias2icuName.Add("CP1363",		  "IBM-1363_P110-1997");
			alias2icuName.Add("IBM1363",	  "IBM-1363_P110-1997");

			//Korean EBCDIC MBCS
			alias2icuName.Add("1364",		  "IBM-1364_P110-1997");
			alias2icuName.Add("CP1364",		  "IBM-1364_P110-1997");
			alias2icuName.Add("IBM1364",	  "IBM-1364_P110-1997");

			//Simplified Chinese ASCII DBCS
			alias2icuName.Add("1380",		  "IBM-1380_P100-1995");
			alias2icuName.Add("CP1380",		  "IBM-1380_P100-1995");
			alias2icuName.Add("IBM1380",	  "IBM-1380_P100-1995");

			//Simplified Chinese ASCII MBCS
			alias2icuName.Add("1381",		  "IBM-1381_P110-1999");
			alias2icuName.Add("CP1381",		  "IBM-1381_P110-1999");
			alias2icuName.Add("IBM1381",	  "IBM-1381_P110-1999");

			//Simplified Chinese (EUC) ASCII DBCS
			alias2icuName.Add("1382",		  "IBM-1382_P100-1995");
			alias2icuName.Add("CP1382",		  "IBM-1382_P100-1995");
			alias2icuName.Add("IBM1382",	  "IBM-1382_P100-1995");

			//Simplified Chinese (EUC) ASCII MBCS
			alias2icuName.Add("1383",		  "IBM-1383_P110-1999");
			alias2icuName.Add("CP1383",		  "IBM-1383_P110-1999");
			alias2icuName.Add("IBM1383",	  "IBM-1383_P110-1999");

			//Simplified Chinese ASCII DBCS
			alias2icuName.Add("1385",		  "IBM-1385_P100-1997");
			alias2icuName.Add("CP1385",		  "IBM-1385_P100-1997");
			alias2icuName.Add("IBM1385",	  "IBM-1385_P100-1997");

			//Simplified Chinese ASCII MBCS
			alias2icuName.Add("1386",		  "IBM-1386_P100-2001");
			alias2icuName.Add("CP1386",		  "IBM-1386_P100-2001");
			alias2icuName.Add("IBM1386",	  "IBM-1386_P100-2001");

			//Simplified Chinese EBCDIC MBCS
			alias2icuName.Add("1388",		  "IBM-1388_P110-2000");
			alias2icuName.Add("CP1388",		  "IBM-1388_P110-2000");
			alias2icuName.Add("IBM1388",	  "IBM-1388_P110-2000");

			//Japanese (Latin-Kanji) EBCDIC MBCS
			alias2icuName.Add("1399",		  "IBM-1399_P110-2003");
			alias2icuName.Add("CP1399",		  "IBM-1399_P110-2003");
			alias2icuName.Add("IBM1399",	  "IBM-1399_P110-2003");

			//Korean EBCDIC DBCS
			alias2icuName.Add("4930",		  "IBM-4930_P100-1997");
			alias2icuName.Add("CP4930",		  "IBM-4930_P100-1997");
			alias2icuName.Add("IBM4930",	  "IBM-4930_P100-1997");

			//Simplified Chinese EBCDIC DBCS
			alias2icuName.Add("4933",		  "IBM-4933_P100-2002");
			alias2icuName.Add("CP4933",		  "IBM-4933_P100-2002");
			alias2icuName.Add("IBM4933",	  "IBM-4933_P100-2002");

			//Japanese (Extended Katakana) EBCDIC MBCS
			alias2icuName.Add("5026",		  "IBM-5026_P120-1999");
			alias2icuName.Add("CP5026",		  "IBM-5026_P120-1999");
			alias2icuName.Add("IBM5026",	  "IBM-5026_P120-1999");

			//Japanese (Extended English) EBCDIC MBCS
			alias2icuName.Add("5035",		  "IBM-5035_P120-1999");
			alias2icuName.Add("CP5035",		  "IBM-5035_P120-1999");
			alias2icuName.Add("IBM5035",	  "IBM-5035_P120-1999");

			//Japanese (HP) ASCII MBCS
			alias2icuName.Add("5039",		  "IBM-5039_P110-1996");
			alias2icuName.Add("CP5039",		  "IBM-5039_P110-1996");
			alias2icuName.Add("IBM5039",	  "IBM-5039_P110-1996");

			//Japanese (Latin-Kanji) EBCDIC DBCS
			alias2icuName.Add("16684",		  "IBM-16684_P110-2003");
			alias2icuName.Add("CP16684",	  "IBM-16684_P110-2003");
			alias2icuName.Add("IBM16684",	  "IBM-16684_P110-2003");
			#endregion
			
		}

		// maps code page # to IANA name
		private static Dictionary<int,string> cp2Name = new Dictionary<int, string>();
		// maps IANA to code page #
		private static Dictionary<string, int> name2Cp = new Dictionary<string, int>();
		// maps alias to IANA name
		private static Dictionary<string, string> alias2Name = new Dictionary<string, string>();
		// maps code page # to ICU map name
		private static Dictionary<int, string> cp2icuName = new Dictionary<int, string>();
		// maps ICU map name to code page #
		private static Dictionary<string, int> icuName2Cp = new Dictionary<string, int>();
		// maps alias to ICU map name
		private static Dictionary<string, string> alias2icuName = new Dictionary<string, string>();
	}
}
