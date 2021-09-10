----------------------------------------------------------------------------------------------------------------
-- Table numbers
----------------------------------------------------------------------------------------------------------------
ORD				1
ORD_L			2
Addresses		3
Empleyees		4
Goods			5
Vhcl			6
TM_GROUPS		11
Addresses_MM	21
CONT_C			101		(Cont_OE_Ord_L és az õt reprezentáló BorderoMegb sor együtt)
INV				201
INV_L			202
WRHS_ORDERS		301
WRHS_ORDERS_L	302
CSHBK			501
CSHBK_L			502
BANK			601
BANK_L			602

----------------------------------------------------------------------------------------------------------------
-- Alkalmazottak	-> Employees
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		BeosztasID							Cust_Positions_ID,
		BeosztasPontosMegn					Cust_Position_Descr,
		Nev									[Name],
		Lakcim_Orszag						Home_Cntry,
		Lakcim_State						Home_State,
		Lakcim_ISZ							Home_ZIP,
		Lakcim_Hsg							Home_Cty,
		Lakcim_Utca							Home_Addr,
		Levcim_Orszag						Mail_Cntry,
		Levcim_State						Mail_State,
		Levcim_ISZ							Mail_ZIP,
		Levcim_Hsg							Mail_Cty,
		Levcim_Utca							Mail_Addr,
		Telefon1							Phone1,
		Telefon2							Phone2,
		Fax									Fax,
		Email								Email,
		TelOtthon1							Other_Phone1,
		TelOtthon2							Other_Phone2,
		Adoazonosito						TaxNum,
		TAJszam								Insurance_Num1,
		TBszam								Insurance_Num2,
		AnyjaNeve							MothersName,
		LeanykoriNeve						MadenName,
		SzulDatum							BirthDate,
		SzulHely							BirthPlace,
		NettoFiz							Wages1,
		BruttoFiz							Wages2,
		TBjarulek							Insurance_Premium,
		Nyugdijpenztar						Pension_Plan,
		NyugdijpenztarPenzforg				Pension_BankCode,
		NyugdijpenztarBankszla				Pension_B,
		NameInFremdsprache1					Linkcode_1,
		NameInFremdsprache2					Linkcode_2,
		VezetoiVizsgaErv					DriverLicenseExpDate,
		TIRVizsgaErv						TIR_Exam_ExpDate,
		UtlevelErv							Passport_ExpDate,
		ADRVizsgaErv						Hazard_Exam_ExpDate,
		SzemelyiIgazolvany					ID_Card_Num,
		KilepettIN							Quit_YN,
		KilepDatum							Quit_Date,
		BelepDatum							Hiring_Date,
		MunkavallaloiEngErvenyes			WorkLicense_ExpDate,
		EgyebOkmanyErvenyes					Other_License_ExpDate,
		Cegek_ID							Cust_ID
		--CegekNev1
  FROM Alkalmazottak	Employees


----------------------------------------------------------------------------------------------------------------
-- Bank	-> Bank
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		BankKontoID							Bank_AC_ID,
		Period								Acct_Periods_ID,
		LNr									DocNum,
		BelegDatum							DocDate,
		OffnungsSaldo						OpeningBalance,
		SchlussSaldo						ClosingBalance,
		Tartozik							Pay_Total_FC,
		Kovetel								Inv_Total_FC,
		DokuStand							Status_Codes_ID
		--StatusID
  FROM Bank



----------------------------------------------------------------------------------------------------------------
-- BankBelege	-> Bank_L
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Alsorszam							SeqNum,
		Art									TranType,
		KopfID								Bank_ID,
		ReID								Inv_ID,
		KasseID								CshBk_ID,
		KompID								Comp_Inv_ID,
		[Text]								Descr,
		Cegek_ID							Cust_ID,
		--KundeName1,
		MwStCode							Acct_TaxCodes_ID,
		MwStProzent							Acct_Tax_Percent,
		TartozikNettoFW						Pay_Gross_FC2,
		TartozikMwStFW						Pay_Tax_FC2,
		TartozikFW							Pay_Net_FC2,
		KovetelNettoFW						Inv_Gross_FC2,
		KovetelMwStFW						Inv_Tax_FC2,
		KovetelFW							Inv_Net_FC2,
		Wahrung								Cur_ID,
		TartozikNetto						Pay_Gross_DC,
		TartozikMwSt						Pay_Tax_DC,
		Tartozik							Pay_Net_DC,
		KovetelNetto						Inv_Gross_DC,
		KovetelMwSt							Inv_Tax_DC,
		Kovetel								Inv_Net_DC,
		KuldoSzlaSzam						Cust_Bank_AC,
		Kozlemeny1							Note1,
		KursDatum							Rate_Date_DC,
		KursEinheit							Rate_Unit_DC,
		Kurs								Rate_DC,
		EURKursDatum						Rate_Date_FC,
		EURKursEinheit	 					Rate_Unit_FC,
		EURKurs         				  	Rate_FC,     
		EURTartozikNetto					Pay_Gross_FC,
		EURTartozikMwSt						Pay_Tax_FC,	
		EURTartozik							Pay_Net_FC,
		EURKovetelNetto						Inv_Gross_FC,
		EURKovetelMwSt						Pay_Tax_FC,
		EURKovetel							Inv_Net_FC,
		FIBUKursDatum						Rate_Date_LC,
		FIBUKursEinheit						Rate_Unit_LC,
		FIBUKurs							Rate_LC,     
		FIBUTartozikNetto					Pay_Gross_LC,
		FIBUTartozikMwSt					Pay_Tax_LC,	
		FIBUTartozik						Pay_Net_LC,     
		FIBUKovetelNetto					Inv_Gross_LC,   
		FIBUKovetelMwSt						Pay_Tax_LC,     
		FIBUKovetel 				        Inv_Net_LC,     
		BelegNr								DocNum,
		LeistDatum							DeliveryDate,
		ReferenzNr							RefNum
FROM	BankBelege


----------------------------------------------------------------------------------------------------------------
-- BankBelege_FIBU	-> Bank_L_Acct
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		ParentRecordID						ParentRecordID,
		FIBUDimensionen_ID					Acct_Views_ID,
		ParentKonto_ID						Parent_GLNums_ID,
		--ParentKontonummer,
		KontoID								GLNums_ID,
		--Kontonummer,
		Fix									Amount,
		FixPercent							Perc,
		BelongTo_ID							BelongsTo_ID,
		--BelongTo_Name,
		Param1								Param1,
		Param2								Param2,
		Param3								Param3
  FROM BankBelege_FIBU


----------------------------------------------------------------------------------------------------------------
-- Bankkontos	-> Bank_ACC
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Benennung							Descr,
		FIBUKonto							Acct_GLNums_ID,
		Bankleitzahl						Bank_AC1,
		BankkontoNr							Bank_AC2,
		IBANCode							Bank_AC3,
		SWIFT								Bank_AC4,
		BankName1							Bank_Name1,
		BankName2							Bank_Name2,
		BankStrasse							Bank_Addr,
		BankLand							Bank_Country,
		BankState							Bank_State,
		BankPLZ								Bank_ZIP,
		BankOrt								Bank_City,
		BankTel1							Bank_Tel1,
		BankTel2							Bank_Tel2,
		BankFax								Bank_Fax,
		BankkontoClosed						Closed,
		ZusatzInt01							UserFld_int01,
		ZusatzInt02							UserFld_int02,
		ZusatzInt03							UserFld_int03,
		ZusatzFloat01						UserFld_float01,
		ZusatzFloat02						UserFld_float02,
		ZusatzFloat03						UserFld_float03,
		ZusatzVarchar01						UserFld_nvarchar01,
		ZusatzVarchar02						UserFld_nvarchar02,
		ZusatzVarchar03						UserFld_nvarchar03,
		ZusatzDate01						UserFld_date01,
		ZusatzDate02						UserFld_date02,
		ZusatzDate03						UserFld_date03
  FROM Bankkontos


----------------------------------------------------------------------------------------------------------------
-- Beosztasok	-> Cust_Positions
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Beosztas							Name
FROM	Beosztasok		Cust_Positions


----------------------------------------------------------------------------------------------------------------
-- Beszelgetesek	-> Cust_CRM
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Cegek_ID							Cust_ID,
		Datum								[Date],
		Megjegyzes							Notes,
		Visszaterni							FollowUp_YN,
		VisszateresDatuma					FollowUp_Date,
		Dokumentum							Attachement
FROM	Beszelgetesek	Cust_CRM


----------------------------------------------------------------------------------------------------------------
-- Bordero	-> Ord
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		HivatkozottBordID					Ref_Ord_ID,
		Offer_Num							Offer_Num,
		Offer_Valid_FROM					Offer_Valid_FROM,
		Offer_Valid_TO						Offer_Valid_TO,
		JaratID								Routes_ID,
		BordNr_1							OrderNum_1,
		BordNr_2							OrderNum_2,
		BordNr_3							OrderNum_3,
		BordNr_4							OrderNum_4,
		BordNr_5							OrderNum_5,
		Fictive								Fictive,
		ProcessCode_1						ProcessCode_1,
		BejovoBordNr						BOL_Num_1,
		Megj								Note1,
		RefNr_1								RefNum_1,
		RefNr_2								RefNum_2,
		RefNr_3								RefNum_3,
		RefNr_4								RefNum_4,
		RefNr_5								RefNum_5,
		Sped1ID								P14_ID,
		Sped1Nev1							P14_Name1,
		Sped1Nev2							P14_Name2,
		Sped1Utca							P14_Addr,
		Sped1Orszag							P14_Country,
		Sped1State							P14_State,
		Sped1ISZ							P14_ZIP,
		Sped1Helyseg						P14_City,
		Sped2ID								P15_ID,
		Sped2Nev1							P15_Name1,
		Sped2Nev2							P15_Name2,
		Sped2Utca							P15_Addr,
		Sped2Orszag							P15_Country,
		Sped2State							P15_State,
		Sped2ISZ							P15_ZIP,
		Sped2Helyseg						P15_City,
		FoAlvID								P16_ID,
		FoAlvNev1							P16_Name1,
		FoAlvNev2							P16_Name2,
		FoAlvUtca							P16_Addr,
		FoAlvOrszag							P16_Country,
		FoAlvState							P16_State,
		FoAlvISZ							P16_ZIP,
		FoAlvHelyseg						P16_City,
		FoAlvKontaktID						P16_CUS_Contacts_ID,
		FoAlvKontakt						P16_Contact,
		FoAlvFax							P16_Fax,
		FoAlvTel							P16_Phone,
		FoAlvEMail							P16_email,
		--FoAlvFuvDij,						Nem kell
		--FoAlvFuvDijDev,					Nem kell
		--FoAlvFuvDijBelf,					Nem kell
		--FoAlvFuvDijArfCode,				Nem kell
		--FoAlvFuvDijMegj,					Nem kell
		--FoAlvFizNap,						Nem kell
		FoAlvJvezID							P16_Driver_Empl_ID,
		FoAlvJvez							P16_Driver_Name,
		P16_Driver_LicenseNum				P16_Driver_LicenseNum,
		P16_Driver_Tel						P16_Driver_Tel,
		P16_Driver_Email					P16_Driver_Email,
		P16_Driver_Skype					P16_Driver_Skype,
		FoAlvSzlaInstrukcio					P16_Inv_Note,
		FoTGK_ID							P16_Truck_ID,
		FoTGKRendszam						P16_Truck_LicenseNum,
		FoTGKTipus							P16_Truck_Type,
		FoTGK_liter							P16_Truck_Mileage,
		FoPKCS_ID							P16_Trailer_ID,
		FoPKCS_liter						P16_Trailer_Mileage,
		FoPKCS_Rendszam						P16_Trailer_LicenseNum,
		FoPKCS_Tipus						P16_Trailer_Type,
		Rakjegyzekszam						BOL_Num_2,
		Datum								[Date],
		FelvetelDatum						Added_Date,
		TulajdonosUserID					Added_Users_ID,
		--UserID2,							Nem kell
		--UserID3,							Nem kell
		UtolsoModDatum						LastModified_Date,
		FelhMezo1							UserFld_nvarchar01,
		FelhMezo2							UserFld_nvarchar02,
		FelhMezo3							UserFld_nvarchar03,
		FelhMezo4							UserFld_nvarchar04,
		FelhMezo5							UserFld_nvarchar05,
		FelhMezo6							UserFld_nvarchar06,
		Datum1								LoadingDate_FROM,
		Datum2								LoadingDate_TO,
		Datum3								UnloadingDate_FROM,
		Datum4								UnloadingDate_TO,
		Datum5								UserFld_Date05,
		Homerseklet							Temperature,
		FVamID								Custom1_ID,
		FVamNev1							Custom1_Name1,
		FVamNev2							Custom1_Name2,
		FVamUtca							Custom1_Addr,
		FVamOrszag							Custom1_Country,
		FVamState							Custom1_State,
		FVamISZ								Custom1_ZIP,
		FVamHelyseg							Custom1_City,
		LVamID								Custom2_ID,
		LVamNev1							Custom2_Name1,
		LVamNev2							Custom2_Name2,
		LVamUtca							Custom2_Addr,
		LVamOrszag							Custom2_Country,
		LVamState							Custom2_State,
		LVamISZ								Custom2_ZIP,
		LVamHelyseg							Custom2_City,
		OsszKM								Distance_Total,
		RakottKM							Distance_Loaded,
		UresKM								Distance_Empty_1,
		UresKM2								Distance_Empty_2,
		KezdoKM								Odometer_Start,
		VegKM								Odometer_End,
		Megj1								Note2,
		Rel1								Rel1,
		Rel2								Rel2,
		Rel3								Rel3,
		StatusID							StatusID,
		FoMegbID							P19_ID,
		FoMegbNev1							P19_Name1,
		FoMegbNev2							P19_Name2,
		FoMegbUtca							P19_Addr,
		FoMegbOrszag						P19_Country,
		FoMegbState							P19_State,
		FoMegbISZ							P19_ZIP,
		FoMegbHelyseg						P19_City,
		FoMegbKontaktID						P19_CUS_Contacts_ID,
		FoMegbKontakt						P19_Contact,
		FoMegbFax							P19_Fax,
		FoMegbTel							P19_Tel,
		FoMegbEMail							P19_email,
		--FoMegbFuvDij,						Nem kell
		--FoMegbFuvDijDev,					Nem kell
		--FoMegbFuvDijMegj,					Nem kell
		FoMegbMegrendelesSzam				P19_OrderNum,
		FoMegbSzlaInstrukcio				P19_Inv_Note,
		SchiffName							Ship_Name,
		SchiffAbfahrt						Ship_ETS,
		SchiffAnkunft						Ship_ETA,
		VoyageNr							VoyageNr,
		BordDok1							UserFld_Doc1,
		BordDok2							UserFld_Doc2,
		BordDok3							UserFld_Doc3,
		BordDok4							UserFld_Doc4,
		BordDok5							UserFld_Doc5,
		Datum6								UserFld_Date6,
		Datum7								UserFld_Date7,
		Datum8								UserFld_Date8,
		BordZona							Zone,
		BordNrInEinFeld						OrderNum,
		BordStatusFlags						Status_Flags,
		--Closed,							Nem kell
		--ClosedUserID,						Nem kell
		Period								ACC_Periods_ID,
		ParitasID							DeliveryTerms_ID,
		Paritas								DeliveryTerms,
		ParitasHelyseg						DeliveryTerms_Dest,
		ContainerType						ContainerType,
		ContainerNum						ContainerNum,
		MrnNum								MrnNum,
		Manually							Manually,
		Type_of_cost_allocation				Type_of_cost_allocation,
		TemplateName						TemplateName,
		Vhcl_TypeID							Vhcl_Types_ID,
		Vhcl_MaxVolume						Vhcl_MaxVolume,
		Vhcl_MaxWeight						Vhcl_MaxWeight,
		Vhcl_Floorspace						Vhcl_Floorspace
  FROM Bordero		Ord


----------------------------------------------------------------------------------------------------------------
-- BorderoAlPoz	-> Ord_Places
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Alsorszam							SeqNum,
		AruID								Ord_Goods_ID,
		BordID								Ord_ID,
		MegbID								Ord_L_ID,
		TypID								TypeID,
		HivatkozasiSzam						RefNum,
		FelLeID								CUST_ID,
		FelLeNev1							CUST_Name1,
		FelLeNev2							CUST_Name2,
		FelLeUtca							CUST_Addr,
		FelLeOrszag							CUST_Country,
		FelLeState							CUST_State,
		FelLeISZ							CUST_ZIP,
		FelLeHelyseg						CUST_City,
		FelLeAdoszam						CUST_TaxNum,
		FelLeFaxszam						CUST_Fax,
		FelLeMegj							CUST_Note,
		FelLeDatumTOL						Date_FROM,
		FelLeDatumIG						Date_TO,
		StatusID							StatusID,
		FelLeVamID							Customs_ID,
		FelLeVamNev1						Customs_Name1,
		FelLeVamNev2						Customs_Name2,
		FelLeVamUtca						Customs_Addr,
		FelLeVamOrszag						Customs_Country,
		FelLeVamState						Customs_State,
		FelLeVamISZ							Customs_ZIP,
		FelLeVamHelyseg						Customs_City,
		Direkt								Direct,
		ZusatzInt01							UserFld_int01,
		ZusatzInt02							UserFld_int02,
		ZusatzInt03							UserFld_int03,
		ZusatzInt04							UserFld_int04,
		ZusatzInt05							UserFld_int05,
		ZusatzInt06							UserFld_int06,
		ZusatzInt07							UserFld_int07,
		ZusatzInt08							UserFld_int08,
		ZusatzInt09							UserFld_int09,
		ZusatzInt10							UserFld_int10,
		ZusatzFloat01						UserFld_float01,
		ZusatzFloat02						UserFld_float02,
		ZusatzFloat03						UserFld_float03,
		ZusatzFloat04						UserFld_float04,
		ZusatzFloat05						UserFld_float05,
		ZusatzFloat06						UserFld_float06,
		ZusatzFloat07						UserFld_float07,
		ZusatzFloat08						UserFld_float08,
		ZusatzFloat09						UserFld_float09,
		ZusatzFloat10						UserFld_float10,
		ZusatzDate01						UserFld_date01,
		ZusatzDate02						UserFld_date02,
		ZusatzDate03						UserFld_date03,
		ZusatzDate04						UserFld_date04,
		ZusatzDate05						UserFld_date05,
		ZusatzDate06						UserFld_date06,
		ZusatzDate07						UserFld_date07,
		ZusatzDate08						UserFld_date08,
		ZusatzDate09						UserFld_date09,
		ZusatzDate10						UserFld_date10,
		ZusatzVarchar01						UserFld_nvarchar01,
		ZusatzVarchar02						UserFld_nvarchar02,
		ZusatzVarchar03						UserFld_nvarchar03,
		ZusatzVarchar04						UserFld_nvarchar04,
		ZusatzVarchar05						UserFld_nvarchar05,
		ZusatzVarchar06						UserFld_nvarchar06,
		ZusatzVarchar07						UserFld_nvarchar07,
		ZusatzVarchar08						UserFld_nvarchar08,
		ZusatzVarchar09						UserFld_nvarchar09,
		ZusatzVarchar10						UserFld_nvarchar10
  FROM BorderoAlPoz		Ord_Places


----------------------------------------------------------------------------------------------------------------
-- BorderoKalk	-> Ord_Charges
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		BordID								Ord_ID,
		MegbID								Ord_L_ID,
		Ord_Good_ID							Ord_Goods_ID,
		CalcType							CalcType,
		ResztvevoID							Part_ID,
		FZID								Cust_ID,
		--FZName1							Nem kell,
		KiadasBevetel						Cr_Dr,
		Alsorszam							SeqNum,
		TarifID								Rates_ID,
		Megnevezes							Descr,
		FWStkPreis							UnitPrice_FC,
		Darab								Pcs,
		ME									Unit,
		FWNetto								Netto_FC,
		Wahrung								Curr_ID,
		MwStCode							ACC_TaxCodes_ID,
		TarifBemerkung						Note,
		Inv_PerformanceDate					Inv_PerformanceDate,
		Inv_CurrencyDate					Inv_CurrencyDate,
		Inv_Status							Inv_Status,
		Calculated							Calculated,
		Closed								Closed,
		ZusatzInt01							UserFld_int01,
		ZusatzInt02							UserFld_int02,
		ZusatzInt03							UserFld_int03,
		ZusatzFloat01						UserFld_float01,
		ZusatzFloat02						UserFld_float02,
		ZusatzFloat03						UserFld_float03,
		ZusatzVarchar01						UserFld_nvarchar01,
		ZusatzVarchar02						UserFld_nvarchar02,
		ZusatzVarchar03						UserFld_nvarchar03,
		ZusatzDate01						UserFld_date01,
		ZusatzDate02						UserFld_date02,
		ZusatzDate03						UserFld_date03
  FROM BorderoKalk


----------------------------------------------------------------------------------------------------------------
-- BorderoKalk_FIBU	-> Ord_Charges_ACC
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		ParentRecordID						ParentRecordID,
		FIBUDimensionen_ID					Acct_Views_ID,
		ParentKonto_ID						Parent_GLNums_ID,
		--ParentKontonummer					Nem kell,
		KontoID								GLNums_ID,
		--Kontonummer						Nem kell,
		Fix									Amount,
		FixPercent							Perc,
		BelongTo_ID							BelongsTo_ID,
		--BelongTo_Name						Nem kell,
		Param1								Param1,
		Param2								Param2,
		Param3								Param3
  FROM BorderoKalk_FIBU		Ord_Charges_ACC

----------------------------------------------------------------------------------------------------------------
-- BorderoMegb	-> Ord_L
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		BorderoID							Ord_ID,
		Alsorszam							SeqNum,
		EvidNr_1							EvidNum_1,
		EvidNr_2							EvidNum_2,
		EvidNr_3							EvidNum_3,
		EvidNr_Slash						EvidNr_Slash,
		EvidNr_Stage						EvidNr_Stage,
		Evid_Stage_SeqNr					Evid_Stage_SeqNum,
		Evid_Parent_Stage_ID				Evid_Parent_Stage_ID,
		EvidNrInOneField					EvidNumInOneField,
		Fictive_Ord_ID						Fictive_Ord_ID,
		URefNr_1							RefNum_1,
		URefNr_2							RefNum_2,
		URefNr_3							RefNum_3,
		URefNr_4							RefNum_4,
		URefNr_5							RefNum_5,
		MegbID								P_26_ID,
		MegbNev1							P_26_Name1,
		MegbNev2							P_26_Name2,
		MegbUtca							P_26_Addr,
		MegbOrszag							P_26_Country,
		MegbState							P_26_State,
		MegbISZ								P_26_ZIP,
		MegbHelyseg							P_26_City,
		--MegbFuvDij						Nem kell,
		--MegbFuvDijDev						Nem kell,
		--MegbFuvDijBelf					Nem kell,
		--MegbFuvDijArfCode					Nem kell,
		--MegbFuvDijMegj					Nem kell,
		MegbKontaktID						P_26_CUS_Contacts_ID,
		MegKontakt							P_26_Contact,
		MegbTel								P_26_Tel,
		MegbFax								P_26_Fax,
		MegbEMail							P_26_email,
		MegbMegj							P_26_Note,
		--MegbFizNap						Nem kell,
		MegrendelesSzam						P_26_OrderNum,
		AlvID								P_23_ID,
		AlvNev1								P_23_Name1,
		AlvNev2								P_23_Name2,
		AlvUtca								P_23_Addr,
		AlvOrszag							P_23_Country,
		AlvState							P_23_State,
		AlvISZ								P_23_ZIP,
		AlvHelyseg							P_23_City,
		AlvKontaktID						P_23_CUS_Contacts_ID,
		AlvKontakt							P_23_Contact,
		AlvFax								P_23_Fax,
		AlvTel								P_23_Tel,
		AlvEMail							P_23_email,
		--AlvFuvDij,
		--AlvFuvDijDev,
		--AlvFuvDijMegj,
		AlvMegj								P_19_Note,
		AlvJvezID							P_19_Driver_Empl_ID,
		AlvJvez								P_19_Driver,
		AlvJvezIgazolvanyszam				P_19_Driver_LicenseNum,
		TGK_ID								P_19_Truck_ID,
		Rendszam							P_19_Truck_LicenseNum,
		TGKTipus							P_19_Truck_Type,
		PKCS_ID								P_19_Trailer_ID,
		PKCS_Rendszam						P_19_Trailer_LicenseNum,
		PKCS_Tipus							P_19_Trailer_Type,
		NyomtatasiJelek						PrintStatusCodes,
		ParitasID							DeliveryTerms_ID,
		Paritas								DeliveryTerms,
		ParitasHelyseg						DeliveryTerms_Dest,
		Rel1								Rel1,
		Rel2								Rel2,
		Rel3								Rel3,
		TulajdonosUserID					Added_Users_ID,
		FelhMezo1							UserFld_01,
		FelhMezo2							UserFld_02,
		FelhMezo3							UserFld_03,
		FelhMezo4							UserFld_04,
		FelhMezo5							UserFld_05,
		FelhMezo6							UserFld_06,
		FelrakVamID							P_24_ID,
		FelrakVamNev1						P_24_Name1,
		FelrakVamNev2						P_24_Name2,
		FelrakVamUtca						P_24_Addr,
		FelrakVamOrszag						P_24_Country,
		FelrakVamState						P_24_State,
		FelrakVamISZ						P_24_ZIP,
		FelrakVamHelyseg					P_24_City,
		FelrakVamMegj						P_24_Note,
		LerakVamID							P_25_ID,
		LerakVamNev1						P_25_Name1,
		LerakVamNev2						P_25_Name2,
		LerakVamUtca						P_25_Addr,
		LerakVamOrszag						P_25_Country,
		LerakVamState						P_25_State,
		LerakVamISZ							P_25_ZIP,
		LerakVamHelyseg						P_25_City,
		LerakVamMegj						P_25_Note,
		FeladoID							P21_ID,
		FeladoNev1							P21_Name1,
		FeladoNev2							P21_Name2,
		FeladoUtca							P21_Addr,
		FeladoOrszag						P21_Country,
		FeladoState							P21_State,
		FeladoISZ							P21_ZIP,
		FeladoHelyseg						P21_City,
		CimzettID							P22_ID,
		CimzettNev1							P22_Name1,
		CimzettNev2							P22_Name2,
		CimzettUtca							P22_Addr,
		CimzettOrszag						P22_Country,
		CimzettState						P22_State,
		CimzettISZ							P22_ZIP,
		CimzettHelyseg						P22_City,
		Sped3ID								P27_ID,
		Sped3Nev1							P27_Name1,
		Sped3Nev2							P27_Name2,
		Sped3Utca							P27_Addr,
		Sped3Orszag							P27_Country,
		Sped3State							P27_State,
		Sped3ISZ							P27_ZIP,
		Sped3Helyseg						P27_City,
		VamDatum							Customs_Date,
		KiszallDatum						DeliveryDate_FROM,
		FelrakDatum							LoadingDate_FROM,
		LerakDatum							UnloadingDate_FROM,
		SzlaInstrukcio						P26_InvoiceNote,
		StatusID							StatusID,
		FelrakDatumIG						LoadingDate_TO,
		LerakDatumIG						UnloadingDate_TO,
		Date1								Date01,
		Date2								Date02,
		Nyitvatartas						BusinessHours,
		Direct								Direct,
		Vinculalt							Vinc,
		KiszallFelrakDatum					Delivery_LoadingDate_FROM,
		KiszallFelrakDatumIG				Delivery_LoadingDate_TO,
		KiszallDatumIG						DeliveryDate_TO,
		FelvetelDatumMegb					Added_Date,
		UtolsoModDatumMegb					LastModified_Date,
		BordMegbOsszKM						Distance_Total,
		BordMegbRakottKM					Distance_Loaded,
		BordMegbUresKM						Distance_Empty_1,
		BordMegbUresKM2						Distance_Empty_2,
		BordMegbKezdoKM						Odometer_Start,
		BordMegbVegKM						Odometer_End,
		Zona								Zone,
		BordMegbDok1						UserFld_Doc01,
		BordMegbDok2						UserFld_Doc02,
		BordMegbDok3						UserFld_Doc03,
		BordMegbDok4						UserFld_Doc04,
		BordMegbDok5						UserFld_Doc05,
		BordNrInEinFeld						OrderNum,
		PosNrInEinFeld						OrderNumWithSeqNum,
		MegbStatusFlags						Status_Flags,
		--MegbStatusJelleg					Nem kell,
		--BordMegbKM						Nem kell,
		LgrBewID							Wrhs_Tran_ID,
		ProcessCode01						ProcessCode01,
		ProcessCode02						ProcessCode02,
		ProcessCode03						ProcessCode03,
		Closed								Closed,
		Manually							Manually,
		ZusatzInt01							UserFld_int01,
		ZusatzInt02							UserFld_int02,
		ZusatzInt03							UserFld_int03,
		ZusatzInt04							UserFld_int04,
		ZusatzInt05							UserFld_int05,
		ZusatzInt06							UserFld_int06,
		ZusatzInt07							UserFld_int07,
		ZusatzInt08							UserFld_int08,
		ZusatzInt09							UserFld_int09,
		ZusatzInt10							UserFld_int10,
		ZusatzFloat01						UserFld_float01,
		ZusatzFloat02						UserFld_float02,
		ZusatzFloat03						UserFld_float03,
		ZusatzFloat04						UserFld_float04,
		ZusatzFloat05						UserFld_float05,
		ZusatzFloat06						UserFld_float06,
		ZusatzFloat07						UserFld_float07,
		ZusatzFloat08						UserFld_float08,
		ZusatzFloat09						UserFld_float09,
		ZusatzFloat10						UserFld_float10,
		ZusatzDate01						UserFld_date01,
		ZusatzDate02						UserFld_date02,
		ZusatzDate03						UserFld_date03,
		ZusatzDate04						UserFld_date04,
		ZusatzDate05						UserFld_date05,
		ZusatzDate06						UserFld_date06,
		ZusatzDate07						UserFld_date07,
		ZusatzDate08						UserFld_date08,
		ZusatzDate09						UserFld_date09,
		ZusatzDate10						UserFld_date10,
		ZusatzVarchar01						UserFld_nvarchar01,
		ZusatzVarchar02						UserFld_nvarchar02,
		ZusatzVarchar03						UserFld_nvarchar03,
		ZusatzVarchar04						UserFld_nvarchar04,
		ZusatzVarchar05						UserFld_nvarchar05,
		ZusatzVarchar06						UserFld_nvarchar06,
		ZusatzVarchar07						UserFld_nvarchar07,
		ZusatzVarchar08						UserFld_nvarchar08,
		ZusatzVarchar09						UserFld_nvarchar09,
		ZusatzVarchar10						UserFld_nvarchar10
  FROM BorderoMegb		Ord_L


----------------------------------------------------------------------------------------------------------------
-- Cegek	-> Cust
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		SzamlazasiCim						InvoiceAddress_ID,
		SzCimTipus							InvoiceAddress_Type,
		Nyitvatartas						BusinessHours,
		RovidNev							ShortName,
		Nev1								Name1,
		Nev2								Name2,
		Orszag								Country,
		[State]								[State],
		ISZ									ZIP,
		Varos								City,
		Utca								Addr,
		SzNev1								Inv_Name1,
		SzNev2								Inv_Name2,
		SzOrszag							Inv_Country,
		SzState								Inv_State,
		SzISZ								Inv_ZIP,
		SzVaros								Inv_City,
		SzUtca								Inv_Addr,
		Fopartner							MainContact_ID,
		RaktarFopartner						Wrhs_Contact_ID,
		LevelcimOrszag						Mail_Country,
		LevelcimState						Mail_State,
		LevelcimISZ							Mail_ZIP,
		LevelcimVaros						Mail_City,
		LevelcimUtca						Mail_Addr,
		Adoszam								TaxNumber1,
		EURAdoszam							TaxNumber2,
		LicenseNumber						LicenseNumber,
		PenzforgJelz						BankCode,
		Szlaszam							BankAccount,
		IBANCode							IBAN,
		Telefon1							Phone1,
		Telefon2							Phone2,
		Telefon3							Phone3,
		Fax1								Fax1,
		Fax2								Fax2,
		--Modem								Nem kell
		eMail								Email,
		Internet							HomePage,
		PartnerOsztaly						Cust_Groups_ID,
		--M2MC								Nem kell
		--M2UtolsoValtoztatas				Nem kell
		FIBUKontoKred						ACC_GLNums_Credit_ID,
		FIBUKontoDeb						ACC_GLNums_Debit_ID,
		Sprache								Lang,
		ClassID1							ClassID1,
		Wahrung								Credit_Curr_ID,
		ArfCode								Debit_Curr_Code,
		TeljDatumCode1						Debit_DeliveryDate_Code,
		TeljDatumCode3						Debit_DeliveryDate_Periodic_Code,
		FizHat1								Debit_DueDays,
		FizMod								Debit_PaymentMethod,
		FizHat2								Credit_DueDays,
		FizMod2								Credit_PaymentMethod,
		ClassID2							ClassID2,
		Wahrung2							Credit_Curr_ID,
		ArfCode2							Credit_Curr_Code,
		TeljDatumCode2						Credit_DeliveryDate_Code,
		TeljDatumCode4						Credit_DeliveryDate_Periodic_Code,
		--ISOpontszam						Nem kell
		UgyfelJeleKonyvelesben				ACCT_Cust_ID,
		MegbizoIN							Customer_YN,
		AlvallalkozoIN						Carrier_YN,
		PSpedIN								Subcontractor_YN,
		CimIN								AddressOnly_YN,
		Megjegyzesek						Notes,
		SajatCegPartnerID					Consultant_Empl_ID,
		TelepNev1							Site_Name1,
		TelepNev2							Site_Name2,
		TelepOrszag							Site_Country,
		TelepState							Site_State,
		TelepISZ							Site_ZIP,
		TelepVaros							Site_City,
		TelepUtca							Site_Addr,
		MCFremdsystem1						LinkCode_1,
		MCFremdsystem2						LinkCode_2,
		Zona								Zone,
		CreditOsszegHUF						CreditLimit_LC,
		CreditOsszegEUR						CreditLimit_FC,
		Discount							Discount,
		Inv_FormatID						Inv_FormatID,
		UseOwnTariff						UseOwnRates,
		M2UtolsoValtoztatas_UserID			LastModified_Users_ID,
		Felvevo_UserID						Added_Users_ID,
		Felvetel_Datuma						Added_Date,
		ZusatzInt01							UserFld_int01,
		ZusatzInt02							UserFld_int02,
		ZusatzInt03							UserFld_int03,
		ZusatzInt04							UserFld_int04,
		ZusatzInt05							UserFld_int05,
		ZusatzInt06							UserFld_int06,
		ZusatzInt07							UserFld_int07,
		ZusatzInt08							UserFld_int08,
		ZusatzInt09							UserFld_int09,
		ZusatzInt10							UserFld_int10,
		ZusatzInt11							UserFld_int11,
		ZusatzInt12							UserFld_int12,
		ZusatzFloat01						UserFld_float01,
		ZusatzFloat02						UserFld_float02,
		ZusatzFloat03						UserFld_float03,
		ZusatzFloat04						UserFld_float04,
		ZusatzFloat05						UserFld_float05,
		ZusatzFloat06						UserFld_float06,
		ZusatzFloat07						UserFld_float07,
		ZusatzFloat08						UserFld_float08,
		ZusatzFloat09						UserFld_float09,
		ZusatzFloat10						UserFld_float10,
		ZusatzVarchar01						UserFld_nvarchar01,
		ZusatzVarchar02						UserFld_nvarchar02,
		ZusatzVarchar03						UserFld_nvarchar03,
		ZusatzVarchar04						UserFld_nvarchar04,
		ZusatzVarchar05						UserFld_nvarchar05,
		ZusatzVarchar06						UserFld_nvarchar06,
		ZusatzVarchar07						UserFld_nvarchar07,
		ZusatzVarchar08						UserFld_nvarchar08,
		ZusatzVarchar09						UserFld_nvarchar09,
		ZusatzVarchar10						UserFld_nvarchar10,
		ZusatzDate01						UserFld_date01,
		ZusatzDate02						UserFld_date02,
		ZusatzDate03						UserFld_date03,
		ZusatzDate04						UserFld_date04,
		ZusatzDate05						UserFld_date05,
		ZusatzDate06						UserFld_date06,
		ZusatzDate07						UserFld_date07,
		ZusatzDate08						UserFld_date08,
		ZusatzDate09						UserFld_date09,
		ZusatzDate10						UserFld_date10
  FROM Cegek	Cust


----------------------------------------------------------------------------------------------------------------
-- Cegek_Price	-> Cust_Prices
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Cegek_ID							Cust_ID,
		LgrTariffType_ID					Wrhs_Rates_Types_ID,
		ValidFROM							ValidFROM,
		ValidTO								ValidTO,
		UserFld_Int01						UserFld_int01,
		UserFld_Int02						UserFld_int02,
		UserFld_Int03						UserFld_int03,
		UserFld_Float01						UserFld_float01,
		UserFld_Float02						UserFld_float02,
		UserFld_Float03						UserFld_float03,
		UserFld_Varchar01					UserFld_nvarchar01,
		UserFld_Varchar02					UserFld_nvarchar02,
		UserFld_Varchar03					UserFld_nvarchar03,
		UserFld_Datetime01					UserFld_date01,
		UserFld_Datetime02					UserFld_date02,
		UserFld_Datetime03					UserFld_date03
FROM	Cegek_Price		Cust_Prices


----------------------------------------------------------------------------------------------------------------
-- CegekEng	-> Cust_Disc
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Cegek_ID							Cust_ID,
		Alsorszam							SeqNum,
		TarifID								Rates_ID,
		ProzEng								Discount_Percentage
FROM	CegekEng		Cust_Discounts


----------------------------------------------------------------------------------------------------------------
-- CegekKalk	-> Cust_Rates
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Cegek_ID							Cust_ID,
		ResztvevoID							Part_ID,
		KiadasBevetel						CrDr,
		Alsorszam							SeqNum,
		TarifID								Rates_ID,
		FWStkPreis							UnitPrice_FC,
		Darab								Pcs,
		Wahrung								Curr_ID,
		MwStCode							ACC_TaxCodes_ID,
		FN_Pcs								FN_Pcs,
		FN_UnitPrice						FN_UnitPrice
FROM	CegekKalk		Cust_Rates


----------------------------------------------------------------------------------------------------------------
-- CegekProperties	-> Cust_Props
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Cegek_ID							Cust_ID,
		ZusatzInt01							UserFld_int01,
		ZusatzInt02							UserFld_int02,
		ZusatzInt03							UserFld_int03,
		ZusatzInt04							UserFld_int04,
		ZusatzInt05							UserFld_int05,
		ZusatzInt06							UserFld_int06,
		ZusatzInt07							UserFld_int07,
		ZusatzInt08							UserFld_int08,
		ZusatzInt09							UserFld_int09,
		ZusatzInt10							UserFld_int10,
		ZusatzFloat01						UserFld_float01,
		ZusatzFloat02						UserFld_float02,
		ZusatzFloat03						UserFld_float03,
		ZusatzFloat04						UserFld_float04,
		ZusatzFloat05						UserFld_float05,
		ZusatzFloat06						UserFld_float06,
		ZusatzFloat07						UserFld_float07,
		ZusatzFloat08						UserFld_float08,
		ZusatzFloat09						UserFld_float09,
		ZusatzFloat10						UserFld_float10,
		ZusatzVarchar01						UserFld_nvarchar01,
		ZusatzVarchar02						UserFld_nvarchar02,
		ZusatzVarchar03						UserFld_nvarchar03,
		ZusatzVarchar04						UserFld_nvarchar04,
		ZusatzVarchar05						UserFld_nvarchar05,
		ZusatzVarchar06						UserFld_nvarchar06,
		ZusatzVarchar07						UserFld_nvarchar07,
		ZusatzVarchar08						UserFld_nvarchar08,
		ZusatzVarchar09						UserFld_nvarchar09,
		ZusatzVarchar10						UserFld_nvarchar10,
		ZusatzDate01						UserFld_date01,
		ZusatzDate02						UserFld_date02,
		ZusatzDate03						UserFld_date03,
		ZusatzDate04						UserFld_date04,
		ZusatzDate05						UserFld_date05,
		ZusatzDate06						UserFld_date06,
		ZusatzDate07						UserFld_date07,
		ZusatzDate08						UserFld_date08,
		ZusatzDate09						UserFld_date09,
		ZusatzDate10						UserFld_date10
  FROM CegekProperties		Cust_Props



----------------------------------------------------------------------------------------------------------------
-- Country	-> Cntr
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Country_Code						Code,
		Country_Description					Descr
FROM Country


----------------------------------------------------------------------------------------------------------------
-- Country_Provinces	-> Cntr_Prv
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Country_ID							Country_ID,
		PR_Code								Code,
		PR_Description						Descr
FROM	Country_Provinces


----------------------------------------------------------------------------------------------------------------
-- Csomagolasok	-> Wrhs_Units
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Jel									Code,
		Megnevezes							Descr,
		ZusatzInt01,
		ZusatzInt02,
		ZusatzInt03,
		ZusatzFloat01,
		ZusatzFloat02,
		ZusatzFloat03,
		ZusatzVarchar01,
		ZusatzVarchar02,
		ZusatzVarchar03,
		ZusatzDate01,
		ZusatzDate02,
		ZusatzDate03
  FROM Csomagolasok


----------------------------------------------------------------------------------------------------------------
-- Dispo1	-> SysTbl_Temp
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		ProcessID							ProcessID,
		Terminal							Terminal,
		Art									Code,
		LNr									SeqNum,
		RecordID							RecordID,
		ExemplarNr							Copies,
		AnzExemplar							CopyNum,
		KZ									Selected,
		Dispo1Float,
		Dispo1Datum,
		Dispo2Float,
		Dispo2Datum,
		Dispo3Float,
		Dispo4Float,
		Dispo5Float,
		Dispo6Float,
		Dispo7Float,
		Dispo8Float,
		Dispo1Varchar,
		Dispo2Varchar,
		Dispo3Varchar,
		Dispo4Varchar,
		Dispo1Long,
		Dispo2Long,
		Dispo3Long,
		Dispo4Long,
		Dispo5Long,
		Dispo6Long,
		Dispo7Long,
		Dispo8Long,
		Dispo1Text,
		Dispo2Text,
		--MultiSelState						Nem kell
  FROM Dispo1



----------------------------------------------------------------------------------------------------------------
-- Doc_Images	-> Doc_Images
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Doc_Types_ID,
		Descr,
		[Message],
		Origin,
		Sent_date,
		Scan_Date,
		Scan_Users_ID,
		StatusID,
		Barcode01,
		Barcode02,
		Barcode03,
		Cegek_ID,
		Doc_Date,
		Doc_Attached,
		Doc_Security_Level
FROM	Doc_Images


----------------------------------------------------------------------------------------------------------------
-- Doc_Pages	-> Doc_Pages
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Doc_Images_ID						Doc_Images_ID,
		PageNum								PageNum,
		Origin								Origin,
		[File_ID]							[File_ID],
		ImageData							ImageData,
		Barcode01							Barcode01,
		Barcode02							Barcode02,
		Barcode03							Barcode03
FROM	Doc_Pages			Doc_Pages


----------------------------------------------------------------------------------------------------------------
-- Doc_Parents	-> Doc_Parents
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Doc_Images_ID						Doc_Images_ID,
		ParentTableName						ParentTableName,
		ParentRecord_ID						ParentRecord_ID,
		ParentRecord_Desc					ParentRecord_Desc
FROM Doc_Parents		Doc_Parents


----------------------------------------------------------------------------------------------------------------
-- Doc_Types	-> Doc_Types
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Descr								Descr,
		SecurityLevel						SecurityLevel,
		Barcode01							Barcode01,
		Barcode02							Barcode02,
		Barcode03							Barcode03
FROM	Doc_Types			Doc_Types


----------------------------------------------------------------------------------------------------------------
-- DoFilter_Params	-> DoFilter_Params
----------------------------------------------------------------------------------------------------------------
SELECT	ProcessID				ProcessID,
		Terminal				Terminal,
		FilterWHERE				FilterWHERE,
		FilterTEXT				FilterTEXT,
		ParamInt00				ParamInt00,
		ParamInt01				ParamInt01,
		ParamInt02				ParamInt02,
		ParamInt03				ParamInt03,
		ParamInt04				ParamInt04,
		ParamInt05				ParamInt05,
		ParamInt06				ParamInt06,
		ParamInt07				ParamInt07,
		ParamInt08				ParamInt08,
		ParamInt09				ParamInt09,
		ParamInt10				ParamInt10,
		ParamDbl00				ParamDbl00,
		ParamDbl01				ParamDbl01,
		ParamDbl02				ParamDbl02,
		ParamDbl03				ParamDbl03,
		ParamDbl04				ParamDbl04,
		ParamDbl05				ParamDbl05,
		ParamDbl06				ParamDbl06,
		ParamDbl07				ParamDbl07,
		ParamDbl08				ParamDbl08,
		ParamDbl09				ParamDbl09,
		ParamDbl10				ParamDbl10,
		ParamStr00				ParamStr00,
		ParamStr01				ParamStr01,
		ParamStr02				ParamStr02,
		ParamStr03				ParamStr03,
		ParamStr04				ParamStr04,
		ParamStr05				ParamStr05,
		ParamStr06				ParamStr06,
		ParamStr07				ParamStr07,
		ParamStr08				ParamStr08,
		ParamStr09				ParamStr09,
		ParamStr10				ParamStr10,
		InsertDate				InsertDate
  FROM DoFilter_Params


----------------------------------------------------------------------------------------------------------------
-- FIBUDimensionen	-> Acct_Views
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Bezeichnung,
		FullFill
  FROM FIBUDimensionen


----------------------------------------------------------------------------------------------------------------
-- FIBUKonten	-> Acct_GLNums
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Kontonummer							ACC_Num,
		Bezeichnung							Descr,
		Wahrung								Curr_ID,
		Saldo								Balance,
		OffnungsSaldo						Opening_Balance,
		MinusBuchungNichtErlaubt			No_Negative_Balance,
		KontoArt							Acct_Views_ID,
		ParentKonto_ID						Parent_Acct_GLNums_ID,
--		ParentKontonummer,
		MCFremdsystem1						Linkcode_1,
		MCFremdsystem2						Linkcode_2
  FROM FIBUKonten		Acct_GLNums


----------------------------------------------------------------------------------------------------------------
-- FIBUSteuerCodes	-> Acct_TaxCodes
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Kurzbezeichnung						Code,
		Bezeichnung							Descr,
		SteuerProzent						Tax_Rate,
		FIBUKonto							ACCT_GLNum,
		MCFremdsystem1						Linkcode_1,
		MCFremdsystem2						Linkcode_2
FROM	FIBUSteuerCodes	Acct_TaxCodes


----------------------------------------------------------------------------------------------------------------
-- FixText	-> ParmTbl_Defs
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Code,
		[Text],
		Titel
  FROM FixText


----------------------------------------------------------------------------------------------------------------
-- FixText0	-> SysTbl_Defs
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Code,
		[Text],
		Titel
  FROM FixText0


----------------------------------------------------------------------------------------------------------------
-- Frankatur	-> DTerms
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		FrankaturName						ShortName,
		LongName							LongName,
		S1,
		S2,
		Falv,
		FMeg,
		Megb,
		Alv,
		Fel,
		Cim,
		S3,
		BoE,
		AlE,
		S1T,
		S2T,
		FalvT,
		FMegT
  FROM Frankatur


----------------------------------------------------------------------------------------------------------------
-- FullKurse	-> SysTbl_CurrRates
----------------------------------------------------------------------------------------------------------------
SELECT Kurse_ID,
		Wahrungen_ID,
		KursDatum,
		KursPro,
		Kurs,
		KursEURO,
		OrigDatum,
		OrigDatumEURO,
		Norm_KursEinheit,
		Norm_Kurs,
		Norm_EURKursEinheit,
		Norm_EURKurs
  FROM FullKurse


----------------------------------------------------------------------------------------------------------------
-- FullKurse_AllDatum	-> SysTbl_CurrDates
----------------------------------------------------------------------------------------------------------------
SELECT Datum
  FROM FullKurse_AllDatum


----------------------------------------------------------------------------------------------------------------
-- FunctionLocks	-> SysTbl_FLocks
----------------------------------------------------------------------------------------------------------------
SELECT FunctionID,
		Terminal
  FROM FunctionLocks


----------------------------------------------------------------------------------------------------------------
-- Holidays	-> Holidays
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		[Date]
		[Description]
  FROM Holidays


----------------------------------------------------------------------------------------------------------------
-- IDs	-> SysTbl_IDs
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		V1
  FROM IDs


----------------------------------------------------------------------------------------------------------------
-- IndKoltseg	-> Rts_Costs
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		IndulasID,
		Ref_Document_ID,
		LineType_ID,
		KiadasBevetel,
		Orszag,
		Company_ID,
		Company_Name_1,
		InvoiceNo,
		Tarifen_ID,
		Tarifen_Description,
		GrossTotal,
		Currency_ID,
		VAT_ID,
		KartyaID,
		Megjegyzes,
		HutoLiter,
		Liter,
		TeljDatum,
		Ref_Tarifen_ID,
		Inv_Status,
		GKV2
  FROM IndKoltseg


----------------------------------------------------------------------------------------------------------------
-- IndKoltseg_FIBU	-> Rts_Costs_Acct
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		ParentRecordID						ParentRecordID,
		FIBUDimensionen_ID					Acct_Views_ID,
		ParentKonto_ID						Parent_GLNums_ID,
		--ParentKontonummer,
		KontoID								GLNums_ID,
		--Kontonummer,
		Fix									Amount,
		FixPercent							Perc,
		BelongTo_ID							BelongsTo_ID,
		--BelongTo_Name,
		Param1								Param1,
		Param2								Param2,
		Param3								Param3
  FROM IndKoltseg_FIBU

----------------------------------------------------------------------------------------------------------------
-- IndMenetlevel	-> Rts_Sheets
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		IndulasID							Routes_ID,
		IndPozicioID						ORD_L_ID,
		Datum								RowDate,
		LNr									SeqNum,
		Helyseg								City_FROM,
		Helyseg2							City_TO,
		Ureskm								Distance_Empty,
		Rakottkm							Distance_Loaded,
		Tonna								[Weight]
  FROM IndMenetlevel


----------------------------------------------------------------------------------------------------------------
-- IndOkmany	-> Rts_Docs
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		IndulasID,
		OkmanyID,
		OkmanyMegnevezes,
		KiadDatum,
		Megjegyzes,
		StatusID,
		AtadasID,
		LNr
  FROM IndOkmany


----------------------------------------------------------------------------------------------------------------
-- IndOkmanyFajta	-> Rts_DocTypes
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		TipusID,
		Megnevezes
  FROM IndOkmanyFajta


----------------------------------------------------------------------------------------------------------------
-- IndOkmanyTorzs	-> Rts_Docs_Base
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		TipusID,
		KiadoOrszag,
		IndOkmanyFajta_ID,
		OkmSzama,
		FelvetDatum,
		ErvenyesDatum,
		StatusID,
		LeadDatum,
		IndulasID
  FROM IndOkmanyTorzs


----------------------------------------------------------------------------------------------------------------
-- IndPozicio	-> Rts_Parents
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		IndulasID							Rts_ID,
		BordID								ParentRecord_ID,
		LNr									SeqNum
		--BordSzam
		--Relation
FROM	IndPozicio		Rts_Parents


----------------------------------------------------------------------------------------------------------------
-- IndTankTorzs	-> Rts_Cards
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		KartyaMegnevezes,
		HMegnevezes,
		Jarmuvek_ID
  FROM IndTankTorzs


----------------------------------------------------------------------------------------------------------------
-- Indulas	-> Rts
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		JaratSzam1							RTS_Num_1,
		JaratSzam2							RTS_Num_2,
		JaratSzam3							RTS_Num_3,
		JaratSzam4							RTS_Num_4,
		JaratSzam5							RTS_Num_5,
		RefID,
		PeriodusEv,
		PeriodusHo,
		Hova,
		FoGkID,
		FoGkRendszam,
		PotGkID,
		PotGkRendszam,
		Sofor1ID,
		SoforNev1,
		Sofor2ID,
		SoforNev2,
		FelrakDatum,
		LerakDatum,
		IndDatum							StartDate,
		ErkDatum							EndDate,
		KmInd,
		KmErk,
		TankInd,
		TankErk,
		HTankInd,
		HTankErk,
		HOraInd,
		HOraErk,
		HutoUzema,
		FizTav,
		Terkep,
		AlapFizetes,
		AlapFizetes_GKV2,
		KmPenz,
		KmPenz_GKV2,
		AlapUa,
		Norma,
		APEHUa,
		APEHNorma,
		Fizkm,
		ServiceKM,
		Allas,
		HutoSzaz,
		ReferenciaUaAr,
		APEHUaAr,
		HatarKiOrszag,
		HatarKiVaros,
		HatarKiDatum,
		HatarBeOrszag,
		HatarBeVaros,
		HatarBeDatum,
		EltOra1,
		EltOra2,
		EltNap1,
		EltNap2,
		Napok,
		ArfolyamDat,
		NapidijArfDatum,
		EgysegDeviza,
		NapidijDevizaID,
		NapidijDeviza,
		NapidijHUF,
		NapidijEUR,
		KursDatum,
		KursEinheit,
		Kurs,
		EURKursDatum,
		EURKursEinheit,
		EURKurs,
		HatarKiOrszag2,
		HatarKiVaros2,
		HatarKiDatum2,
		HatarBeOrszag2,
		HatarBeVaros2,
		HatarBeDatum2,
		EltOra12,
		EltOra22,
		EltNap12,
		EltNap22,
		Napok2,
		NapidijArfDatum2,
		EgysegDeviza2,
		NapidijDeviza2,
		NapidijHUF2,
		NapidijEUR2,
		KursDatum2,
		KursEinheit2,
		Kurs2,
		EURKursDatum2,
		EURKursEinheit2,
		EURKurs2,
		Closed,
		JaratszamInEinFeld					RTS_Num,
		FelvetelDatum						Added_Date,
		StatusID,
		ElszamolasDatum,
		ElszamoltatoID,
		ZusatzInt01,
		ZusatzInt02,
		ZusatzInt03,
		ZusatzInt04,
		ZusatzInt05,
		ZusatzInt06,
		ZusatzInt07,
		ZusatzInt08,
		ZusatzInt09,
		ZusatzInt10,
		ZusatzFloat01,
		ZusatzFloat02,
		ZusatzFloat03,
		ZusatzFloat04,
		ZusatzFloat05,
		ZusatzFloat06,
		ZusatzFloat07,
		ZusatzFloat08,
		ZusatzFloat09,
		ZusatzFloat10,
		ZusatzVarchar01,
		ZusatzVarchar02,
		ZusatzVarchar03,
		ZusatzVarchar04,
		ZusatzVarchar05,
		ZusatzVarchar06,
		ZusatzVarchar07,
		ZusatzVarchar08,
		ZusatzVarchar09,
		ZusatzVarchar10,
		ZusatzDate01,
		ZusatzDate02,
		ZusatzDate03,
		ZusatzDate04,
		ZusatzDate05,
		ZusatzDate06,
		ZusatzDate07,
		ZusatzDate08,
		ZusatzDate09,
		ZusatzDate10
  FROM Indulas


----------------------------------------------------------------------------------------------------------------
-- JarmuCsoportok	-> Vhcl_Grp
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		KurzBenennung						Name
FROM	JarmuCsoportok


----------------------------------------------------------------------------------------------------------------
-- Jarmuvek	-> Vhcl
----------------------------------------------------------------------------------------------------------------
SELECT	ID										ID,
		TransactID								TransactID,
		JarmuCsoportID							VHCL_GR_ID,
		TGK_PKCS								TT_YN,
		PKCS_ID									Trailer_VHCL_ID,
		Rendszam								PlateNum,
		BelsoAzonosito							FleetNum,
		Tipus									VHCL_Type,
		Felepitmeny								Equipment,
		Alkalmazottak_ID						EMPL_ID,
		GyartasiDatum							ProductionDate,
		BeszerzesDatuma							PurchaseDate,
		EladasiDatum							SoldDate,
		MegengedettOsszSuly						AllowedGrossWeight,
		HasznosRaksuly							UsefulWeight,
		Rakterfogat								Volume,
		LM										FloorSpace,
		KoltsegPerKM							OperatingCostDistance,
		KoltsegPerNap							OperatingCostDay,
		ZoldkartyaErvenyes						EmmissionsTestExpDate,
		VizsgaErvenyes							InspectionCertificatDate,
		ADRervenyes								HazmitCertificatDate,
		ADReng									HazmitCertificatNum,
		KotelezoBiztDija						InsuranceCost,
		GkgAdo									LicenseFee,
		CertifikatErvenyes						CertificatDate,
		KoltseghelyID							CostCenter_ID,
		Alvazszam								VINNum,
		Motorszam								EngineNum,
		Onsuly									NetWeight,
		Terhelhetoseg							LoadWeight,
		PKCSBelmagassag							TrailerHeight,
		PKCSSzelesseg							TrailerWigth,
		PKCSHosszusag							TrailerLength,
		Eladva									IsSold,
		EladDatum								SoldDate,
		UAnorma									MileageAllowance,
		AlvID									Subcontractor_ID,
		Norma									Norm,
		Potlek1									NormAllowance1,
		Potlek2									NormAllowance2,
		Potlek3									NormAllowance3,
		Potlek4									NormAllowance4,
		Potlek5									NormAllowance5,
		APEHNorma								OfficialNorm,
		APEHPotlek1								OfficialNormAllowance1,
		APEHPotlek2								OfficialNormAllowance2,
		APEHPotlek3								OfficialNormAllowance3,
		APEHPotlek4								OfficialNormAllowance4,
		APEHPotlek5								OfficialNormAllowance5,
		OlajcsereKmAllas						OilChangeMileage,
		FekcsereKmAllas							BrakeReplacementMileage,
		GumicsereKmAllas						TiresReplacementMileage,
		ZusatzInt01								UserField_Int01,
		ZusatzInt02								UserField_Int02,
		ZusatzInt03								UserField_Int03,
		ZusatzFloat01							UserField_Float01,
		ZusatzFloat02							UserField_Float02,
		ZusatzFloat03							UserField_Float03,
		ZusatzVarchar01							UserField_Varchar01,
		ZusatzVarchar02							UserField_Varchar02,
		ZusatzVarchar03							UserField_Varchar03,
		ZusatzDate01							UserField_Date01,
		ZusatzDate02							UserField_Date02,
		ZusatzDate03							UserField_Date03
  FROM Jarmuvek


----------------------------------------------------------------------------------------------------------------
-- KasseBuch	-> CshBk
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Period								ACCT_Periods_ID,
		KasseID								CSHBK_Regs_ID,
		FormID								FormID,
		LNr									Cshbk_Num,
		LongLNr								Cshbk_Num_INT,
		KiadasBevetel						CrDr,
		Datum								DocDate,
		Storno								Cancelled,
		AusgestelltVon						Added_Users_ID,
		DokuStand							Doc_Status,
		KundeID								Cust_ID,
		--KundeName1,
		--PersonID,
		Person								Person_Name,
		--VorzahlungAbgerechnet,
		--VorzahlungID,
		Gedruckt							Printed,
		StornoGedruckt						Cancel_Printed,
		AbschlussJN							ClosedYN,
		--AnzBeilagen,
		SaldoNachBeleg						CSHBK_Regs_Balance,
		EndBetrag							Total_DC,
		EndBetragEUR						Total_FC,
		EndBetragFIBU						Total_LC,
		SumTartozik							Pay_Total_DC,
		SumKovetel							Inv_Total_DC,
		SumTartozikEUR						Pay_Total_FC,
		SumKovetelEUR						Inv_Total_FC,
		SumTartozikFIBU						Pay_Total_LC,
		SumKovetelFIBU						Inv_Total_LC,
		OrderNr								Doc_SeqNum
  FROM KasseBuch


----------------------------------------------------------------------------------------------------------------
-- KasseBuchBelege	-> CshBk_L
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		KopfID,
		Alsorszam,
		TransactionTypeID,
		ReID,
		VorzahlungID,
		BankBelegID,
		IndKoltsegID,
		KundeID,
		[Text],
		TartozikFW,
		KovetelFW,
		Wahrung,
		EURTartozik,
		EURKovetel,
		Tartozik,
		Kovetel,
		KursDatum,
		KursEinheit,
		Kurs,
		EURKursDatum,
		EURKursEinheit,
		EURKurs,
		FIBUKursDatum,
		FIBUKursEinheit,
		FIBUKurs,
		FIBUTartozik,
		FIBUKovetel,
		BelegNr,
		ReferenzNr
  FROM KasseBuchBelege

----------------------------------------------------------------------------------------------------------------
-- Kassen	-> CshBk_Regs
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Benennung							Descr,
		FIBUKonto							Acct_GLNumbers_ID,
		LNrSchlusselAus						ParmTbl_Params_KeyOUT,
		LNrSchlusselEin						ParmTbl_Params_KeyIN
FROM	Kassen


----------------------------------------------------------------------------------------------------------------
-- Kurse	-> Cur_Rates
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Wahrungen_ID						Curr_ID,
		KursDatum							Curr_Date,
		KursPro								Curr_Unit,
		Kurs								Curr_LC,
		KursEURO							Curr_FC
  FROM Kurse


----------------------------------------------------------------------------------------------------------------
-- LgrArtGruppen	-> WRHS_GOODS_GROUPS
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Bezeichnung							Name,
		Einlagerungsprinzip					Removal_Method
  FROM LgrArtGruppen	WRHS_GOODS_GROUPS


----------------------------------------------------------------------------------------------------------------
-- LgrArtikel	-> WRHS_ITEMS
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		EinlagererID						Depositor_ID,
		ArtikelNr							ItemNo,
		ArtikelNr2							ItemNo_2,
		ArtikelNr3							ItemNo_3,
		ManufacturerItemNo					ManufacturerItemNo,
		EAN_Code							EAN_Code,
		ITJ									Item_Code,
		GruppeID							WRHS_GOODS_GROUPS_ID,
		Bezeichnung1						Descr_1,
		Bezeichnung2						Descr_2,
		Bezeichnung3						Descr_3,
		AuslagerungsPrinzip					Removal_Method,
		Mindestbestand						Min_Stock,
		ReferenzArtikelID					Ref_WRHS_ITEMS_ID,
		ReferenzArtikelNr					Ref_WRHS_ITEMS_No,
		VPCode								Unit_ID,
		Teilung								Div,
		Pcs_StorageOutUnit					Removal_Unit_Qty,
		StorageOutUnit_PalletLine			Removal_Unit_PalletLine,
		Gewicht								Weight,
		Weight_StorageOutUnit				Removal_Unit_Weight,
		CBM									Volume,
		Height_Normal						Size_Height,
		Width_Normal						Size_Width,
		Lenght_Normal						Size_Length,
		Height_StorageOutUnit				Removal_Unit_Height,
		Width_StorageOutUnit				Removal_Unit_Width,
		Lenght_StorageOutUnit				Removal_Unit_Length,
		Bestand								Stock,
		ResBestand							Reserved_Stock,
		Preis1								Price_1,
		Preis2								Price_2,
		Preis3								Price_3,
		Flache								Area,
		ZollTarifNr							Customs_Tariff_Number,
		UrsprungsLand						CountryOfOrigin,
		DrittLand							ThirdCountry,
		Wahrung1							Curr_1_ID,
		Wahrung2							Curr_2_ID,
		Wahrung3							Curr_3_ID,
		MwStCode1							ACCT_TaxCode_1,
		MwStCode2							ACCT_TaxCode_2,
		MwStCode3							ACCT_TaxCode_3,
		VirtualLagerID						WRHS_Decl_Virt_ID,
		LgrLagerortID						WRHS_Decl_ID,
		LgrStellplatzID						WRHS_Locs_ID,
		LgrStellplatzBenennung				WRHS_Locs_Name,
		Anz									Qty,
		Pal_Height							Pal_Height,
		Stuckliste							Parts_list_YN,
		NotInUse							NotInUse,

		ZusatzInt01							UserFld_IN_int01,
		ZusatzInt02							UserFld_IN_int02,
		ZusatzInt03							UserFld_IN_int03,
		ZusatzFloat01						UserFld_IN_float01,
		ZusatzFloat02						UserFld_IN_float02,
		ZusatzFloat03						UserFld_IN_float03,
		ZusatzVarchar01						UserFld_IN_nvarchar01,
		ZusatzVarchar02						UserFld_IN_nvarchar02,
		ZusatzVarchar03						UserFld_IN_nvarchar03

  FROM LgrArtikel WRHS_ITEMS


----------------------------------------------------------------------------------------------------------------
-- LgrArtikel_EAN	-> WRHS_ITEMS_EAN
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		LgrArtikel_ID						WRHS_ITEMS_ID,
		EAN_Code							EAN_Code,
		ItemNo								ItemNo,
		Cegek_ID							CUST_ID,
		--CompanyName,
		UserFld_Int01						UserFld_Int01,
		UserFld_Int02						UserFld_Int02,
		UserFld_Int03						UserFld_Int03,
		UserFld_Float01						UserFld_Float01,
		UserFld_Float02						UserFld_Float02,
		UserFld_Float03						UserFld_Float03,
		UserFld_Varchar01					UserFld_nvarchar01,
		UserFld_Varchar02					UserFld_nvarchar02,
		UserFld_Varchar03					UserFld_nvarchar03,
		UserFld_Datetime01					UserFld_Datetime01,
		UserFld_Datetime02					UserFld_Datetime02,
		UserFld_Datetime03					UserFld_Datetime03
  FROM LgrArtikel_EAN		WRHS_ITEMS_EAN


----------------------------------------------------------------------------------------------------------------
-- LgrArtikel_Price	-> WRHS_ITEMS_PRICES
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		EinlagererID						Depositor_ID,
		ParentRecordID						ParentRecordID,
		--ArtikelNr,
		PriceType_ID						PriceTypes_ID,
		--LgrTariffType_ID,
		Price,
		Wahrungen_ID						Curr_ID,
		MwStCode							ACCT_Taxcodes_ID,
		ZusatzInt01							UserFld_Int01,
		ZusatzInt02							UserFld_Int02,
		ZusatzInt03							UserFld_Int03,
		ZusatzFloat01						UserFld_Float01,
		ZusatzFloat02						UserFld_Float02,
		ZusatzFloat03						UserFld_Float03,
		ZusatzVarchar01						UserFld_nvarchar01,
		ZusatzVarchar02						UserFld_nvarchar02,
		ZusatzVarchar03						UserFld_nvarchar03,
		ZusatzDate01						UserFld_Datetime01,
		ZusatzDate02						UserFld_Datetime02,
		ZusatzDate03						UserFld_Datetime03

 FROM	LgrArtikel_Price		WRHS_ITEMS_PRICES


----------------------------------------------------------------------------------------------------------------
-- LgrBew	-> Wrhs_Tran
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		TulajdonosUserID					Added_Users_ID,
		EinlagererID						Depositor_ID,
		BewNr								Movement_No,
		VerbuchungID						Booking_ID,
		VerbuchungsDatum					Booking_Date,
		VerbuchungsUserID					Booking_Users_ID,
		KennungEinAusRuckBruch				Movement_Type,
		FremdLieferschein1					Ext_DeliveryNoteNo_1,
		FremdLieferschein2					Ext_DeliveryNoteNo_2,
		LieferDatum							DeliveryDate,
		TerminDatum							Deadline,
		Bemerkungen1						Remarks_1,
		Bemerkungen2						Remarks_2,
		--LieferantID,
		--LieferantName1,
		--LieferantName2,
		--LieferantStrasse,
		--LieferantLand,
		--LieferantState,
		--LieferantPLZ,
		--LieferantOrt,
		VamBelepDatum						Customs_Clearance_Date,
		ContainerNr							Cont_Num,
		BestellDatum						Order_Date,
		--KontaktPerson,
		--KontaktPersonID,
		--AtvevoID,
		--AtvevoName1,
		--AtvevoName2,
		--AtvevoStrasse,
		--AtvevoLand,
		--AtvevoState,
		--AtvevoPLZ,
		--AtvevoOrt,
		--AtvevoPersonID,
		--AtvevoPerson,
		LieferscheinNr						DeliveryNote_No,
		LieferscheinDruckDatum				DeliveryNote_Date,
		LieferscheinFormatID				DeliveryNote_Format_ID,
		StatusID							Statuscodes_ID,
		Szamlak_ID							WRHS_INV_ID,
		--ElolegOsszeg,

		ZusatzInt01							UserFld_Int01,
		ZusatzInt02							UserFld_Int02,
		ZusatzInt03							UserFld_Int03,
		ZusatzInt04							UserFld_Int04,
		ZusatzInt05							UserFld_Int05,
		ZusatzInt06							UserFld_Int06,
		ZusatzInt07							UserFld_Int07,
		ZusatzInt08							UserFld_Int08,
		ZusatzInt09							UserFld_Int09,
		ZusatzInt10							UserFld_Int10,
		ZusatzInt11							UserFld_Int11,
		ZusatzInt12							UserFld_Int12,
		ZusatzFloat01						UserFld_Float01,
		ZusatzFloat02						UserFld_Float02,
		ZusatzFloat03						UserFld_Float03,
		ZusatzFloat04						UserFld_Float04,
		ZusatzFloat05						UserFld_Float05,
		ZusatzFloat06						UserFld_Float06,
		ZusatzFloat07						UserFld_Float07,
		ZusatzFloat08						UserFld_Float08,
		ZusatzFloat09						UserFld_Float09,
		ZusatzFloat10						UserFld_Float10,
		ZusatzVarchar01						UserFld_nvarchar01,
		ZusatzVarchar02						UserFld_nvarchar02,
		ZusatzVarchar03						UserFld_nvarchar03,
		ZusatzVarchar04						UserFld_nvarchar04,
		ZusatzVarchar05						UserFld_nvarchar05,
		ZusatzVarchar06						UserFld_nvarchar06,
		ZusatzVarchar07						UserFld_nvarchar07,
		ZusatzVarchar08						UserFld_nvarchar08,
		ZusatzVarchar09						UserFld_nvarchar09,
		ZusatzVarchar10						UserFld_nvarchar10,
		ZusatzDate01						UserFld_Datetime01,
		ZusatzDate02						UserFld_Datetime02,
		ZusatzDate03						UserFld_Datetime03,
		ZusatzDate04						UserFld_Datetime04,
		ZusatzDate05						UserFld_Datetime05,
		ZusatzDate06						UserFld_Datetime06,
		ZusatzDate07						UserFld_Datetime07,
		ZusatzDate08						UserFld_Datetime08,
		ZusatzDate09						UserFld_Datetime09,
		ZusatzDate10						UserFld_Datetime10
  FROM LgrBew


----------------------------------------------------------------------------------------------------------------
-- LgrInventory	-> WRHS_Inventory
----------------------------------------------------------------------------------------------------------------
SELECT Terminal								Terminal,
		ArtikelID							WRHS_DECL_ID,
		ProdFeld1							Batchnum_1,
		VirtualLagerID						WRHS_DECL_Virt_ID,
		LagerortID							WRHS_DECL_ID,
		StellplatzID						WRHS_LOCS_ID,
		MHD									Expire_Date,
		PalettenNr							PAL_Barcode,
		Bestand								Stock,
		ResBestand							Reserved_Stock,
		ZeilenMitBestand					CoutnOfRowsWithStock,
		Gewicht								Weight,
		CBM									Voluem,
		ZusatzInt01							UserFld_Int01,
		ZusatzInt02							UserFld_Int02,
		ZusatzInt03							UserFld_Int03,
		ZusatzFloat01						UserFld_Float01,
		ZusatzFloat02						UserFld_Float02,
		ZusatzFloat03						UserFld_Float03,
		ZusatzVarchar01						UserFld_nvarchar01,
		ZusatzVarchar02						UserFld_nvarchar02,
		ZusatzVarchar03						UserFld_nvarchar03,
		ZusatzDate01						UserFld_Datetime01,
		ZusatzDate02						UserFld_Datetime02,
		ZusatzDate03						UserFld_Datetime03

  FROM LgrInventory		WRHS_Inventory


----------------------------------------------------------------------------------------------------------------
-- LgrKund	-> WRHS_CUST
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		KundeID								CUST_ID,
		--KundeName1,
		ZusatzInt01							UserFld_Int01,
		ZusatzInt02							UserFld_Int02,
		ZusatzInt03							UserFld_Int03,
		ZusatzFloat01						UserFld_Float01,
		ZusatzFloat02						UserFld_Float02,
		ZusatzFloat03						UserFld_Float03,
		ZusatzVarchar01						UserFld_nvarchar01,
		ZusatzVarchar02						UserFld_nvarchar02,
		ZusatzVarchar03						UserFld_nvarchar03,
		ZusatzDate01						UserFld_Datetime01,
		ZusatzDate02						UserFld_Datetime02,
		ZusatzDate03						UserFld_Datetime03
  FROM LgrKund


----------------------------------------------------------------------------------------------------------------
-- LgrOrte	-> WRHS_Decl
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		KundeID								CUST_ID,
		--KundeName1,
		Benennung_Dim1						Dim_1,
		Benennung_Dim2						Dim_2,
		Benennung_Dim3						Dim_3,
		Benennung_Dim4						Dim_4,
		VirtualLager						Type,
		AutoStorageOut_Closed				AutoStorageOut_Closed
  FROM LgrOrte


----------------------------------------------------------------------------------------------------------------
-- LgrPos	-> WrhsTran_L
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		BewID								WRHS_ID,
		WRHS_ORDERS_L_ID					WRHS_ORDERS_L_ID,
		Stuckliste_LgrPos_ID				Parts_list_ID,
		BewNr								WRHS_Num,
		EinlagererID						Depositor_ID,
		VerbuchungID						Booking_ID,
		VerbuchungsDatum					Booking_Date,
		KennungEinAusRuckBruch				Movement_Type,
		Alsorszam							SeqNum,
		IN_ORD_L_ID							IN_ORD_L_ID,
		IN_ORD_L_TO_GOODS_ID				IN_ORD_L_TO_GOODS_ID,
		OUT_ORD_L_ID						OUT_ORD_L_ID,
		OUT_ORD_L_TO_GOODS_ID				OUT_ORD_L_TO_GOODS_ID,
		PosAruID							ORD_GOODS_ID,
		OldArtikelID						OUT_WRHS_ITEM_ID,
		--OldArtikelNr						OUT_WRHS_ITEM_Num,
		ArtikelID							IN_WRHS_ITEM_ID,
		--ArtikelNr							IN_WRHS_ITEM_Num,
		OldAnz								OUT_Qty,
		Anz									IN_Qty,
		MHD									Expire_Date,
		ProdDatum							Prod_Date,
		OldProdFeld1						OUT_Batchnum_1,
		OldProdFeld2						OUT_Batchnum_2,
		ProdFeld1							IN_Batchnum_1,
		ProdFeld2							IN_Batchnum_2,
		OldPalettenNr						OUT_PAL_Barcode,
		PalettenNr							IN_PAL_Barcode,
		RestAnz								Stock,
		OldPosID							OUT_WRHS_L_ID,
		PosID								IN_WRHS_L_ID,
		StellplatzID						IN_WRHS_Locs_ID,
		--StellplatzBenennung					IN_WRHS_Locs_Name,
		OldStellplatzID						OUT_WRHS_Locs_ID,
		--OldStellplatzBenennung				OUT_WRHS_Locs_Name,
		Gewicht								[Weight],
		CBM									Volume,
		m2									[Space],
		LagerortID							IN_WRHS_Decl_ID,
		OldLagerortID						OUT_WRHS_Decl_ID,
		VirtualLagerID						IN_WRHS_Decl_Virt_ID,
		OldVirtualLagerID					OUT_WRHS_Decl_Virt_ID,
		AktOldBestand						OUT_Item_Stock,
		AktBestand							IN_Item_Stock,
		BelfDatum							CustomsClearance_Date,
		LgrSzamlak_ID						WRHS_INV_ID,
		Preis								UnitPrice_DC,
		Wahrung								Curr_ID,
		Netto								Netto_DC,
		MwStCode							ACCT_Taxcodes_ID,
		MwStProzent							Tax_Percent,
		MwSt								Tax_DC,
		Brutto								Brutto_DC,
		--AktLagerortBestand					Stock_IN_WRHS_Decl_ID,
		--AktOldLagerortBestand				Stock_OUT_WRHS_Decl_ID,
		--AktVirtualLagerBestand				Stock_IN_WRHS_Decl_Virt_ID,
		--AktOldVirtualLagerBestand			Stock_OUT_WRHS_Decl_Virt_ID,
		Ausgebucht							Stock_Movement_Empty_YN,
		Ausgebucht_Datum					Stock_Movement_Empty_Date,
		RootID								RootID,
		Returngoods							Returngoods,
		NecessarySpaceAfterMovement			NecessarySpaceAfterMovement,
		Pallet_Type_ID						Pallet_Type_ID,
		Pallet_Owner_Pcs					Pallet_Owner_Pcs,
		Pallet_Added						Pallet_Added,
		Remarks1							Remarks1,
		Remarks2							Remarks2,
		External_Movement_No				External_Movement_No,
		External_Transport_Order_No			External_Transport_Order_No,
		External_PO_No						External_PO_No,
		External_DeliveryNote_No			External_DeliveryNote_No,
		External_MarksAndNumbers_01			External_MarksAndNumbers_01,
		External_MarksAndNumbers_02			External_MarksAndNumbers_02,
		External_MarksAndNumbers_03			External_MarksAndNumbers_03,
		Barcode_2D_01						Barcode_2D_01,
		Barcode_2D_02						Barcode_2D_02,
		Barcode_2D_03						Barcode_2D_03,
		Barcode_Picking_Pallet				Barcode_Picking_Pallet,
		Barcode_Picking_Box					Barcode_Picking_Box,
		OldZusatzInt01						UserFld_OUT_int01,
		OldZusatzInt02						UserFld_OUT_int02,
		OldZusatzInt03						UserFld_OUT_int03,
		OldZusatzFloat01					UserFld_OUT_float01,
		OldZusatzFloat02					UserFld_OUT_float02,
		OldZusatzFloat03					UserFld_OUT_float03,
		OldZusatzVarchar01					UserFld_OUT_nvarchar01,
		OldZusatzVarchar02					UserFld_OUT_nvarchar02,
		OldZusatzVarchar03					UserFld_OUT_nvarchar03,
		OldZusatzDate01						UserFld_OUT_date01,
		OldZusatzDate02						UserFld_OUT_date02,
		OldZusatzDate03						UserFld_OUT_date03,

		ZusatzInt01							UserFld_IN_int01,
		ZusatzInt02							UserFld_IN_int02,
		ZusatzInt03							UserFld_IN_int03,
		ZusatzFloat01						UserFld_IN_float01,
		ZusatzFloat02						UserFld_IN_float02,
		ZusatzFloat03						UserFld_IN_float03,
		ZusatzVarchar01						UserFld_IN_nvarchar01,
		ZusatzVarchar02						UserFld_IN_nvarchar02,
		ZusatzVarchar03						UserFld_IN_nvarchar03,
		ZusatzDate01						UserFld_IN_date01,
		ZusatzDate02						UserFld_IN_date02,
		ZusatzDate03						UserFld_IN_date03

  FROM LgrPos


----------------------------------------------------------------------------------------------------------------
-- LgrStellplatze	-> Wrhs_Locs
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		LagerortID							Wrhs_Decl_ID,
		Stellplatz1							Dim_1,
		Stellplatz2							Dim_2,
		Stellplatz3							Dim_3,
		Stellplatz4							Dim_4,
		Closed								Closed,
		SortFeld1							SortField_1,
		SortFeld2							SortField_2,
		SortFeld3							SortField_3,
		SortFeld4							SortField_4,
		SortFeld5							SortField_5,
		SortFeld6							SortField_6,
		Besetzt								Reserved,
		NameInEinFeld						LocName,
		KeinBesetzKontrol					No_Reservation_Control,
		MehrArtikelPlatz					Different_Goods_Available,
		Max_Height							Max_Height,
		Max_Weight							Max_Weight,
		ZusatzInt01							UserFld_Int01,
		ZusatzInt02							UserFld_Int02,
		ZusatzInt03							UserFld_Int03,
		ZusatzFloat01						UserFld_Float01,
		ZusatzFloat02						UserFld_Float02,
		ZusatzFloat03						UserFld_Float03,
		ZusatzVarchar01						UserFld_nvarchar01,
		ZusatzVarchar02						UserFld_nvarchar02,
		ZusatzVarchar03						UserFld_nvarchar03,
		ZusatzDate01						UserFld_Datetime01,
		ZusatzDate02						UserFld_Datetime02,
		ZusatzDate03						UserFld_Datetime03
  FROM LgrStellplatze


----------------------------------------------------------------------------------------------------------------
-- LgrStellplatze_Inv	-> WRHS_LOCS_Inventory
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		LgrStellplatze_ID					WRHS_LOCS_ID,
		Alsorszam							SeqNum,
		PosAruID							ORD_GOODS_ID,
		EinlagererID						Depositor_ID,
		ArtikelID							WRHS_ITEMS_ID,
		--ArtikelNr,
		ProdFeld1							Batchnum_1,
		ProdFeld2							Batchnum_2,
		ProdDatum							Prod_Date,
		VirtualLagerID						WRHS_DECL_VIRT_ID,
		Gewicht								Weight,
		CBM									Volume,
		BelfDatum							CustomsClearance_Date,
		MHD									Expire_Date,
		Anz,
		PosID,
		PalettenNr,
		Preis,
		Wahrung,
		Netto,
		MwStCode,
		MwStProzent,
		MwSt,
		Brutto,
		ZusatzInt01							UserFld_Int01,
		ZusatzInt02							UserFld_Int02,
		ZusatzInt03							UserFld_Int03,
		ZusatzFloat01						UserFld_Float01,
		ZusatzFloat02						UserFld_Float02,
		ZusatzFloat03						UserFld_Float03,
		ZusatzVarchar01						UserFld_nvarchar01,
		ZusatzVarchar02						UserFld_nvarchar02,
		ZusatzVarchar03						UserFld_nvarchar03,
		ZusatzDate01						UserFld_Datetime01,
		ZusatzDate02						UserFld_Datetime02,
		ZusatzDate03						UserFld_Datetime03
  FROM LgrStellplatze_Inv


----------------------------------------------------------------------------------------------------------------
-- LgrStucklisten	-> WRHS_PARTS_LISTS
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		ParentRecordID						ParentRecordID,
		Alsorszam							SeqNum,
		LgrArtikel_ID						WRHS_ITEMS_ID,
		--LgrArtikel_ArtikelNr,
		Anz									Qty,
		ZusatzInt01							UserFld_Int01,
		ZusatzInt02							UserFld_Int02,
		ZusatzInt03							UserFld_Int03,
		ZusatzFloat01						UserFld_Float01,
		ZusatzFloat02						UserFld_Float02,
		ZusatzFloat03						UserFld_Float03,
		ZusatzVarchar01						UserFld_nvarchar01,
		ZusatzVarchar02						UserFld_nvarchar02,
		ZusatzVarchar03						UserFld_nvarchar03,
		ZusatzDate01						UserFld_Datetime01,
		ZusatzDate02						UserFld_Datetime02,
		ZusatzDate03						UserFld_Datetime03
  FROM LgrStucklisten	WRHS_PARTS_LISTS


----------------------------------------------------------------------------------------------------------------
-- LgrSzamlak	-> WRHS_INV
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		LgrBewID							WRHS_ID,
		Szamlak_ID							INV_ID,
		SzlaSzam							Inv_No,
		SzallitoKod							Vendor_ID,
		SzallitoNev1						Vendor_Name1,
		SzallitoNev2						Vendor_Name2,
		SzallitoOrszag						Vendor_Country,
		SzallitoState						Vendor_State,
		SzallitoISZ							Vendor_ZIP,
		SzallitoVaros						Vendor_City,
		SzallitoUtca						Vendor_Addr,
		SzallitoAdoszam						Vendor_TaxNum,
		VevoKod								Cust_ID,
		VevoNev1							Cust_Name1,
		VevoNev2							Cust_Name2,
		VevoOrszag							Cust_Country,
		VevoState							Cust_State,
		VevoISZ								Cust_ZIP,
		VevoVaros							Cust_City,
		VevoUtca							Cust_Addr,
		SzallitoAdoszam						Cust_TaxNum,
		Kelte								InvDate,
		FWGesamtNetto						Netto_DC,		-- DC = Document currency
		FWGesamtMwSt						Tax_DC,
		FWGesamtBrutto						Brutto_DC,
		Wahrung								Curr_ID,
		Bemerkung							Remark
--		AuftragsNr
  FROM LgrSzamlak		WRHS_INV


----------------------------------------------------------------------------------------------------------------
-- LgrTariffType	-> WRHS_CHRG_TYPES
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		[Description]						Descr,
		NoDiscount							NoDiscount,
		ZusatzInt01							UserFld_Int01,
		ZusatzInt02							UserFld_Int02,
		ZusatzInt03							UserFld_Int03,
		ZusatzFloat01						UserFld_Float01,
		ZusatzFloat02						UserFld_Float02,
		ZusatzFloat03						UserFld_Float03,
		ZusatzVarchar01						UserFld_nvarchar01,
		ZusatzVarchar02						UserFld_nvarchar02,
		ZusatzVarchar03						UserFld_nvarchar03,
		ZusatzDate01						UserFld_Datetime01,
		ZusatzDate02						UserFld_Datetime02,
		ZusatzDate03						UserFld_Datetime03
FROM	LgrTariffType	WRHS_CHRG_TYPES


----------------------------------------------------------------------------------------------------------------
-- LocalJobs	-> LocalJobs
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Terminal,
		JobDate,
		JobStartingTime,
		JobEndTime,
		[Status],
		OpenArgs,
		Form_Name
  FROM LocalJobs


----------------------------------------------------------------------------------------------------------------
-- Logs_Activity	-> Logs_Activity
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		ActivityStart,
		ActivityEnd,
		NewRecord,
		RecordID,
		Terminal,
		Users_ID,
		TableName,
		FormName
  FROM Logs_Activity


----------------------------------------------------------------------------------------------------------------
-- Logs_RecordLocks	-> Logs_RecordLocks
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Lock_Start,
		Lock_End,
		RecordID,
		Terminal,
		Users_ID,
		TableName,
		FormName
FROM	Logs_RecordLocks


----------------------------------------------------------------------------------------------------------------
-- MSPED2	-> ParmTbl_Texts
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Sprache								Lang,
		Art									[Type],
		Nummer								Number,
		[Text]								[Text],
		LetzteAenderung						LastModified_Date,
		Bemerkungen							Descr1,
		StatusText							Descr2,
		TipText								Descr3
FROM	MSPED2	ParmTbl_Texts



------------------------------------------------------------------------------------------------------
-- Ord_L_To_Goods
------------------------------------------------------------------------------------------------------

-- NEM KELL ALIAS!!!

----------------------------------------------------------------------------------------------------------------
-- Parameterek	-> ParmTbl_Params
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		KulcsElotag							Param_Code_1,
		Kulcs								Param_Code_2,
		NumErtek							Param_Int,
		AlfaNumErtek						Param_Char
  FROM Parameterek


----------------------------------------------------------------------------------------------------------------
-- Parameterek0	-> SysTbl_Params
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		KulcsElotag,
		Kulcs,
		NumErtek,
		AlfaNumErtek
  FROM Parameterek0


----------------------------------------------------------------------------------------------------------------
-- PartnerOsztalyok	-> Cust_Groups
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Megnevezes							Descr
  FROM PartnerOsztalyok


----------------------------------------------------------------------------------------------------------------
-- Perioden	-> Acct_Periods
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Benennung							Descr,
		DatumVON							Date_FROM,
		DatumBIS							Date_TO,
		NeueSendungNichtErfassen			New_Order_not_allowed,
		KasseGeschlossen					CSHBK_Closed,
		AusReGeschlossen					INV_Outgoing_Closed,
		EinReGeschlossen					INV_Incoming_Closed,
		ParentPeriodID						Parent_ACCT_Periods_ID,
		MCFremdsystem						LinkCode_1
FROM	Perioden


------------------------------------------------------------------------------------------------------
-- PozAruk  -> Ord_Goods
------------------------------------------------------------------------------------------------------

SELECT	ID									ID,
		TransactID							TransactID,
		FelID								Loading_Ord_Places_ID,
		LeID								Unloading_Ord_Places_ID,
		AruID								Wrhs_Goods_ID,
		WRHS_ID								Wrhs_Tran_L_ID,
		WRHS_Tran_Num						Wrhs_Tran_Num,
		WRHS_BOL_Num						Wrhs_BOL_Num,
		JelSzam								Descr,
		DbDouble							Pcs_float,
		Db									Pcs,
		CsomID								Packaging_ID,
		Csom								Packaging,
		BelsoDbDouble						Sub_Pcs_float,
		BelsoDB								Sub_Pcs,
		BelsoCsom							Sub_Packaging,
		Tartalom							Descr2,
		--LgrArtikel_ArtikelNr				Nem kell,
		--LgrArtikel_ManufacturerItemNo,	Nem kell
		--LgrArtikel_Bezeichnung1,			Nem kell
		LgrTariffType_ID					Wrhs_RateTypes_ID,
		--LgrTariffType_Description,		Nem kell
		Suly								[Weight],
		Weight_Brutto						Weight_Gross,
		LM									LoadingMeter,
		CBM									Volume,
		m2									Place,
		Deviza								Value_Curr_ID,
		Realization_UnitPrice				Realization_UnitPrice,
		Aruertek							[Value],
		Realization_Acct_TaxCodes_ID		Realization_Acct_TaxCodes_ID,
		VeszelyesIN							Hazardous_YN,
		VeszelyesOsztaly					Hazardous_Class,
		VeszelyesJel						Hazardous_Code,
		VeszelyesMegn						Hazardous_Descr,
		VeszelyesCsoport					Hazardous_Group,
		StatusID							StatusID,
		ContainerTyp						Container_Type,
		ContainerNr							Container_Num,
		Raktar								Wrhs_Decl_ID,
		ZollDatum							Customs_Date,
		Customs_TariffNum					Customs_TariffNum,
		Customs_FolderNum					Customs_FolderNum,
		Bemerkung							Note,
		ElszSuly							EqWeight,
		PalletsNum							EqPcs,
		AruStatusFlags						Status_Flags,
		Size_Width							Size_Width,
		Size_Length							Size_Length,
		Size_Height							Size_Height,
		Pre_Description						Pre_Descr,
		Pre_Pcs_Double						Pre_Pcs_float,
		Pre_Pcs								Pre_Pcs,
		Pre_WRHS_Packaging_ID				Pre_Packaging_ID,
		Pre_Packaging						Pre_Packaging,
		Pre_Sub_Pcs_Double					Pre_Sub_Pcs_float,
		Pre_Sub_Pcs							Pre_Sub_Pcs,
		Pre_Sub_Packaging					Pre_Sub_Packaging,
		Pre_Description2					Pre_Descr2,
		--Pre_LgrArtikel_ArtikelNr			Nem kell,
		--Pre_LgrArtikel_ManufacturerItemNo	Nem kell,
		--Pre_LgrArtikel_Bezeichnung1		Nem kell,
		Pre_LgrTariffType_ID				Pre_LgrTariffType_ID,
		--Pre_LgrTariffType_Description		Nem kell,
		Pre_Weight							Pre_Weight,
		Pre_Weight_Brutto					Pre_Weight_Gross,
		Pre_Space							Pre_LoadingMeter,
		Pre_Volume							Pre_Volume,
		Pre_m2								Pre_Place,
		Pre_Value							Pre_Value,
		Pre_Value_CUR_ID					Pre_Value_Curr_ID,
		Pre_Hazardous_YN					Pre_Hazardous_YN,
		Pre_Hazardous_Class					Pre_Hazardous_Class,
		Pre_Hazardous_Code					Pre_Hazardous_Code,
		Pre_Hazardous_Desc					Pre_Hazardous_Desc,
		Pre_Hazardous_Group					Pre_Hazardous_Group,
		Pre_Container_Type					Pre_Container_Type,
		Pre_EqWeight						Pre_EqWeight,
		Pre_PalletsNum						Pre_EqPcs,
		Pre_Size_Width						Pre_Size_Width,
		Pre_Size_Length						Pre_Size_Length,
		Pre_Size_Height						Pre_Size_Height,
		EKAER								EKAER,
		Manually							Manually,
		ZusatzInt01							UserFld_int01,
		ZusatzInt02							UserFld_int02,
		ZusatzInt03							UserFld_int03,
		ZusatzInt04							UserFld_int04,
		ZusatzInt05							UserFld_int05,
		ZusatzInt06							UserFld_int06,
		ZusatzInt07							UserFld_int07,
		ZusatzInt08							UserFld_int08,
		ZusatzInt09							UserFld_int09,
		ZusatzInt10							UserFld_int10,
		ZusatzFloat01						UserFld_float01,
		ZusatzFloat02						UserFld_float02,
		ZusatzFloat03						UserFld_float03,
		ZusatzFloat04						UserFld_float04,
		ZusatzFloat05						UserFld_float05,
		ZusatzFloat06						UserFld_float06,
		ZusatzFloat07						UserFld_float07,
		ZusatzFloat08						UserFld_float08,
		ZusatzFloat09						UserFld_float09,
		ZusatzFloat10						UserFld_float10,
		ZusatzDate01						UserFld_date01,
		ZusatzDate02						UserFld_date02,
		ZusatzDate03						UserFld_date03,
		ZusatzDate04						UserFld_date04,
		ZusatzDate05						UserFld_date05,
		ZusatzDate06						UserFld_date06,
		ZusatzDate07						UserFld_date07,
		ZusatzDate08						UserFld_date08,
		ZusatzDate09						UserFld_date09,
		ZusatzDate10						UserFld_date10,
		ZusatzVarchar01						UserFld_nvarchar01,
		ZusatzVarchar02						UserFld_nvarchar02,
		ZusatzVarchar03						UserFld_nvarchar03,
		ZusatzVarchar04						UserFld_nvarchar04,
		ZusatzVarchar05						UserFld_nvarchar05,
		ZusatzVarchar06						UserFld_nvarchar06,
		ZusatzVarchar07						UserFld_nvarchar07,
		ZusatzVarchar08						UserFld_nvarchar08,
		ZusatzVarchar09						UserFld_nvarchar09,
		ZusatzVarchar10						UserFld_nvarchar10
  FROM	PozAruk		Ord_Goods




----------------------------------------------------------------------------------------------------------------
-- RecordLocks	-> SysTbl_RLocks
----------------------------------------------------------------------------------------------------------------
SELECT RecordID,
		TableName,
		Terminal,
		FormName
  FROM RecordLocks


----------------------------------------------------------------------------------------------------------------
-- RS	-> RS
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		Terminals_ID,
		Obj_Name
  FROM RS


----------------------------------------------------------------------------------------------------------------
-- RS_Fields	-> RS_Fields
----------------------------------------------------------------------------------------------------------------
SELECT ServerObject_Prefix,
		SubPrefix,
		SeqNum,
		FieldName,
		LabelName,
		CreateAtRuntime,
		CreateAtRuntime_FieldType,
		Field_Top,
		Field_Left,
		Field_Width,
		Field_Height,
		Mondatory,
		Locked,
		Visible,
		xType_VB,
		F_Format,
		DT_FixText_Key,
		DT_WHERE2,
		BG_Color_NORMAL,
		BG_Image,
		BG_Toggle,
		Label_Text,
		Label_Top,
		Label_Left,
		Label_Width,
		Label_Height,
		ShowInGRID,
		SavePoint,
		Forced_NextField
  FROM RS_Fields


----------------------------------------------------------------------------------------------------------------
-- RS_Fields_DEBUG	-> RS_Fields_DEBUG
----------------------------------------------------------------------------------------------------------------
SELECT ServerObject_Prefix,
		SubPrefix,
		SeqNum,
		FieldName,
		LabelName,
		CreateAtRuntime,
		CreateAtRuntime_FieldType,
		Field_Top,
		Field_Left,
		Field_Width,
		Field_Height,
		Mondatory,
		Locked,
		Visible,
		xType_VB,
		F_Format,
		DT_FixText_Key,
		DT_WHERE2,
		BG_Color_NORMAL,
		BG_Image,
		BG_Toggle,
		Label_Text,
		Label_Top,
		Label_Left,
		Label_Width,
		Label_Height,
		ShowInGRID,
		SavePoint,
		Forced_NextField
  FROM RS_Fields_DEBUG


----------------------------------------------------------------------------------------------------------------
-- RS_Fields0	-> RS_Fields0
----------------------------------------------------------------------------------------------------------------
SELECT ServerObject_Prefix,
		SubPrefix,
		SeqNum,
		FieldName,
		LabelName,
		CreateAtRuntime,
		CreateAtRuntime_FieldType,
		Field_Top,
		Field_Left,
		Field_Width,
		Field_Height,
		Mondatory,
		Locked,
		Visible,
		xType_VB,
		F_Format,
		DT_FixText_Key,
		DT_WHERE2,
		BG_Color_NORMAL,
		BG_Image,
		BG_Toggle,
		Label_Text,
		Label_Top,
		Label_Left,
		Label_Width,
		Label_Height,
		ShowInGRID,
		SavePoint,
		Forced_NextField
  FROM RS_Fields0


----------------------------------------------------------------------------------------------------------------
-- RS_L	-> RS_L
----------------------------------------------------------------------------------------------------------------
SELECT RS_ID,
		SeqNum,
		RecordID
  FROM RS_L


----------------------------------------------------------------------------------------------------------------
-- SablonBordero	-> Ord_Temp
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		SablonNev,
		Art,
		HivatkozottBordID,
		JaratID,
		BordNr_1,
		BordNr_2,
		BordNr_3,
		BordNr_4,
		BordNr_5,
		Fictive,
		ProcessCode_1,
		BejovoBordNr,
		Megj,
		RefNr_1,
		RefNr_2,
		RefNr_3,
		RefNr_4,
		RefNr_5,
		Sped1ID,
		Sped1Nev1,
		Sped1Nev2,
		Sped1Utca,
		Sped1Orszag,
		Sped1State,
		Sped1ISZ,
		Sped1Helyseg,
		Sped2ID,
		Sped2Nev1,
		Sped2Nev2,
		Sped2Utca,
		Sped2Orszag,
		Sped2State,
		Sped2ISZ,
		Sped2Helyseg,
		FoAlvID,
		FoAlvNev1,
		FoAlvNev2,
		FoAlvUtca,
		FoAlvOrszag,
		FoAlvState,
		FoAlvISZ,
		FoAlvHelyseg,
		FoAlvKontaktID,
		FoAlvKontakt,
		FoAlvFax,
		FoAlvTel,
		FoAlvEMail,
		FoAlvFuvDij,
		FoAlvFuvDijDev,
		FoAlvFuvDijBelf,
		FoAlvFuvDijArfCode,
		FoAlvFuvDijMegj,
		FoAlvFizNap,
		FoAlvJvezID,
		FoAlvJvez,
		FoAlvSzlaInstrukcio,
		FoTGK_ID,
		FoTGKRendszam,
		FoTGKTipus,
		FoTGK_liter,
		FoPKCS_ID,
		FoPKCS_liter,
		FoPKCS_Rendszam,
		FoPKCS_Tipus,
		Rakjegyzekszam,
		Datum,
		FelvetelDatum,
		TulajdonosUserID,
		UserID2,
		UserID3,
		UtolsoModDatum,
		FelhMezo1,
		FelhMezo2,
		FelhMezo3,
		FelhMezo4,
		FelhMezo5,
		FelhMezo6,
		Datum1,
		Datum2,
		Datum3,
		Datum4,
		Datum5,
		Homerseklet,
		FVamID,
		FVamNev1,
		FVamNev2,
		FVamUtca,
		FVamOrszag,
		FVamState,
		FVamISZ,
		FVamHelyseg,
		LVamID,
		LVamNev1,
		LVamNev2,
		LVamUtca,
		LVamOrszag,
		LVamState,
		LVamISZ,
		LVamHelyseg,
		OsszKM,
		RakottKM,
		UresKM,
		UresKM2,
		KezdoKM,
		VegKM,
		Megj1,
		Rel1,
		Rel2,
		Rel3,
		StatusID,
		FoMegbID,
		FoMegbNev1,
		FoMegbNev2,
		FoMegbUtca,
		FoMegbOrszag,
		FoMegbState,
		FoMegbISZ,
		FoMegbHelyseg,
		FoMegbKontaktID,
		FoMegbKontakt,
		FoMegbFax,
		FoMegbTel,
		FoMegbEMail,
		FoMegbFuvDij,
		FoMegbFuvDijDev,
		FoMegbFuvDijMegj,
		FoMegbMegrendelesSzam,
		FoMegbSzlaInstrukcio,
		SchiffName,
		SchiffAbfahrt,
		SchiffAnkunft,
		BordDok1,
		BordDok2,
		BordDok3,
		BordDok4,
		BordDok5,
		Datum6,
		Datum7,
		Datum8,
		BordZona,
		BordNrInEinFeld,
		BordStatusFlags,
		Closed,
		ClosedUserID,
		Period,
		ParitasID,
		Paritas,
		ParitasHelyseg,
		ContainerType,
		ContainerNum,
		MrnNum,
		Manually,
		Air_Amount_Of_Ins,
		Air_By_01,
		Air_By_02,
		Air_By_03,
		Air_DecVal_For_Carr,
		Air_DecVal_For_Cust,
		Air_FlightDate_01,
		Air_FlightDate_02,
		Air_FlightNr_01,
		Air_FlightNr_02,
		Air_ProcessCode_01,
		Air_SCI_Code,
		Air_To_01,
		Air_To_02,
		Air_To_03
  FROM SablonBordero


----------------------------------------------------------------------------------------------------------------
-- SablonBorderoAlPoz	-> Ord_Temp_Places
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Alsorszam,
		AruID,
		BordID,
		MegbID,
		TypID,
		HivatkozasiSzam,
		FelLeID,
		FelLeNev1,
		FelLeNev2,
		FelLeUtca,
		FelLeOrszag,
		FelLeState,
		FelLeISZ,
		FelLeHelyseg,
		FelLeAdoszam,
		FelLeFaxszam,
		FelLeMegj,
		FelLeDatumTOL,
		FelLeDatumIG,
		StatusID,
		FelLeVamID,
		FelLeVamNev1,
		FelLeVamNev2,
		FelLeVamUtca,
		FelLeVamOrszag,
		FelLeVamState,
		FelLeVamISZ,
		FelLeVamHelyseg,
		Direkt,
		ZusatzInt01,
		ZusatzInt02,
		ZusatzInt03,
		ZusatzInt04,
		ZusatzInt05,
		ZusatzInt06,
		ZusatzInt07,
		ZusatzInt08,
		ZusatzInt09,
		ZusatzInt10,
		ZusatzFloat01,
		ZusatzFloat02,
		ZusatzFloat03,
		ZusatzFloat04,
		ZusatzFloat05,
		ZusatzFloat06,
		ZusatzFloat07,
		ZusatzFloat08,
		ZusatzFloat09,
		ZusatzFloat10,
		ZusatzDate01,
		ZusatzDate02,
		ZusatzDate03,
		ZusatzDate04,
		ZusatzDate05,
		ZusatzDate06,
		ZusatzDate07,
		ZusatzDate08,
		ZusatzDate09,
		ZusatzDate10,
		ZusatzVarchar01,
		ZusatzVarchar02,
		ZusatzVarchar03,
		ZusatzVarchar04,
		ZusatzVarchar05,
		ZusatzVarchar06,
		ZusatzVarchar07,
		ZusatzVarchar08,
		ZusatzVarchar09,
		ZusatzVarchar10
  FROM SablonBorderoAlPoz


----------------------------------------------------------------------------------------------------------------
-- SablonBorderoKalk	-> Ord_Temp_Chrg
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		BordID,
		MegbID,
		Ord_Good_ID,
		CalcType,
		ResztvevoID,
		FZID,
		FZName1,
		KiadasBevetel,
		Alsorszam,
		TarifID,
		Megnevezes,
		FWStkPreis,
		Darab,
		ME,
		FWNetto,
		Wahrung,
		MwStCode,
		TarifBemerkung,
		Inv_PerformanceDate,
		Inv_CurrencyDate,
		Inv_Status,
		Calculated,
		Closed,
		ZusatzInt01,
		ZusatzInt02,
		ZusatzInt03,
		ZusatzFloat01,
		ZusatzFloat02,
		ZusatzFloat03,
		ZusatzVarchar01,
		ZusatzVarchar02,
		ZusatzVarchar03,
		ZusatzDate01,
		ZusatzDate02,
		ZusatzDate03
  FROM SablonBorderoKalk


----------------------------------------------------------------------------------------------------------------
-- SablonBorderoMegb	-> Ord_Temp_L
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		BorderoID,
		Alsorszam,
		EvidNr_1,
		EvidNr_2,
		EvidNr_3,
		EvidNr_Slash,
		EvidNr_Stage,
		Evid_Stage_SeqNr,
		Evid_Parent_Stage_ID,
		EvidNrInOneField,
		Fictive_Ord_ID,
		URefNr_1,
		URefNr_2,
		URefNr_3,
		URefNr_4,
		URefNr_5,
		MegbID,
		MegbNev1,
		MegbNev2,
		MegbUtca,
		MegbOrszag,
		MegbState,
		MegbISZ,
		MegbHelyseg,
		MegbFuvDij,
		MegbFuvDijDev,
		MegbFuvDijBelf,
		MegbFuvDijArfCode,
		MegbFuvDijMegj,
		MegbKontaktID,
		MegKontakt,
		MegbTel,
		MegbFax,
		MegbEMail,
		MegbMegj,
		MegbFizNap,
		MegrendelesSzam,
		AlvID,
		AlvNev1,
		AlvNev2,
		AlvUtca,
		AlvOrszag,
		AlvState,
		AlvISZ,
		AlvHelyseg,
		AlvKontaktID,
		AlvKontakt,
		AlvFax,
		AlvTel,
		AlvEMail,
		AlvFuvDij,
		AlvFuvDijDev,
		AlvFuvDijMegj,
		AlvMegj,
		AlvJvezID,
		AlvJvez,
		AlvJvezIgazolvanyszam,
		TGK_ID,
		Rendszam,
		TGKTipus,
		PKCS_ID,
		PKCS_Rendszam,
		PKCS_Tipus,
		NyomtatasiJelek,
		ParitasID,
		Paritas,
		ParitasHelyseg,
		Rel1,
		Rel2,
		Rel3,
		TulajdonosUserID,
		FelhMezo1,
		FelhMezo2,
		FelhMezo3,
		FelhMezo4,
		FelhMezo5,
		FelhMezo6,
		FelrakVamID,
		FelrakVamNev1,
		FelrakVamNev2,
		FelrakVamUtca,
		FelrakVamOrszag,
		FelrakVamState,
		FelrakVamISZ,
		FelrakVamHelyseg,
		FelrakVamMegj,
		LerakVamID,
		LerakVamNev1,
		LerakVamNev2,
		LerakVamUtca,
		LerakVamOrszag,
		LerakVamState,
		LerakVamISZ,
		LerakVamHelyseg,
		LerakVamMegj,
		FeladoID,
		FeladoNev1,
		FeladoNev2,
		FeladoUtca,
		FeladoOrszag,
		FeladoState,
		FeladoISZ,
		FeladoHelyseg,
		CimzettID,
		CimzettNev1,
		CimzettNev2,
		CimzettUtca,
		CimzettOrszag,
		CimzettState,
		CimzettISZ,
		CimzettHelyseg,
		Sped3ID,
		Sped3Nev1,
		Sped3Nev2,
		Sped3Utca,
		Sped3Orszag,
		Sped3State,
		Sped3ISZ,
		Sped3Helyseg,
		VamDatum,
		KiszallDatum,
		FelrakDatum,
		LerakDatum,
		SzlaInstrukcio,
		StatusID,
		FelrakDatumIG,
		LerakDatumIG,
		Date1,
		Date2,
		Nyitvatartas,
		Direct,
		Vinculalt,
		KiszallFelrakDatum,
		KiszallFelrakDatumIG,
		KiszallDatumIG,
		FelvetelDatumMegb,
		UtolsoModDatumMegb,
		BordMegbOsszKM,
		BordMegbRakottKM,
		BordMegbUresKM,
		BordMegbUresKM2,
		BordMegbKezdoKM,
		BordMegbVegKM,
		Zona,
		BordMegbDok1,
		BordMegbDok2,
		BordMegbDok3,
		BordMegbDok4,
		BordMegbDok5,
		BordNrInEinFeld,
		PosNrInEinFeld,
		MegbStatusFlags,
		MegbStatusJelleg,
		BordMegbKM,
		LgrBewID,
		ProcessCode01,
		ProcessCode02,
		ProcessCode03,
		Closed,
		Manually,
		ZusatzInt01,
		ZusatzInt02,
		ZusatzInt03,
		ZusatzInt04,
		ZusatzInt05,
		ZusatzInt06,
		ZusatzInt07,
		ZusatzInt08,
		ZusatzInt09,
		ZusatzInt10,
		ZusatzFloat01,
		ZusatzFloat02,
		ZusatzFloat03,
		ZusatzFloat04,
		ZusatzFloat05,
		ZusatzFloat06,
		ZusatzFloat07,
		ZusatzFloat08,
		ZusatzFloat09,
		ZusatzFloat10,
		ZusatzDate01,
		ZusatzDate02,
		ZusatzDate03,
		ZusatzDate04,
		ZusatzDate05,
		ZusatzDate06,
		ZusatzDate07,
		ZusatzDate08,
		ZusatzDate09,
		ZusatzDate10,
		ZusatzVarchar01,
		ZusatzVarchar02,
		ZusatzVarchar03,
		ZusatzVarchar04,
		ZusatzVarchar05,
		ZusatzVarchar06,
		ZusatzVarchar07,
		ZusatzVarchar08,
		ZusatzVarchar09,
		ZusatzVarchar10
  FROM SablonBorderoMegb


----------------------------------------------------------------------------------------------------------------
-- SablonPozAruk	-> Ord_Temp_Goods
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		FelID,
		LeID,
		AruID,
		WRHS_ID,
		WRHS_Tran_Num,
		WRHS_BOL_Num,
		JelSzam,
		DbDouble,
		Db,
		CsomID,
		Csom,
		BelsoDbDouble,
		BelsoDB,
		BelsoCsom,
		Tartalom,
		LgrArtikel_ArtikelNr,
		LgrArtikel_ManufacturerItemNo,
		LgrArtikel_Bezeichnung1,
		LgrTariffType_ID,
		LgrTariffType_Description,
		Suly,
		LM,
		CBM,
		m2,
		Deviza,
		Realization_UnitPrice,
		Aruertek,
		Realization_Acct_TaxCodes_ID,
		VeszelyesIN,
		VeszelyesOsztaly,
		VeszelyesJel,
		VeszelyesMegn,
		VeszelyesCsoport,
		StatusID,
		ContainerTyp,
		ContainerNr,
		Raktar,
		ZollDatum,
		Customs_TariffNum,
		Customs_FolderNum,
		Bemerkung,
		ElszSuly,
		PalletsNum,
		AruStatusFlags,
		Pre_Description,
		Pre_Pcs_Double,
		Pre_Pcs,
		Pre_WRHS_Packaging_ID,
		Pre_Packaging,
		Pre_Sub_Pcs_Double,
		Pre_Sub_Pcs,
		Pre_Sub_Packaging,
		Pre_Description2,
		Pre_LgrArtikel_ArtikelNr,
		Pre_LgrArtikel_ManufacturerItemNo,
		Pre_LgrArtikel_Bezeichnung1,
		Pre_LgrTariffType_ID,
		Pre_LgrTariffType_Description,
		Pre_Weight,
		Pre_Space,
		Pre_Volume,
		Pre_m2,
		Pre_Value,
		Pre_Value_CUR_ID,
		Pre_Hazardous_YN,
		Pre_Hazardous_Class,
		Pre_Hazardous_Code,
		Pre_Hazardous_Desc,
		Pre_Hazardous_Group,
		Pre_Container_Type,
		Pre_EqWeight,
		Pre_PalletsNum,
		Manually,
		ZusatzInt01,
		ZusatzInt02,
		ZusatzInt03,
		ZusatzInt04,
		ZusatzInt05,
		ZusatzInt06,
		ZusatzInt07,
		ZusatzInt08,
		ZusatzInt09,
		ZusatzInt10,
		ZusatzFloat01,
		ZusatzFloat02,
		ZusatzFloat03,
		ZusatzFloat04,
		ZusatzFloat05,
		ZusatzFloat06,
		ZusatzFloat07,
		ZusatzFloat08,
		ZusatzFloat09,
		ZusatzFloat10,
		ZusatzDate01,
		ZusatzDate02,
		ZusatzDate03,
		ZusatzDate04,
		ZusatzDate05,
		ZusatzDate06,
		ZusatzDate07,
		ZusatzDate08,
		ZusatzDate09,
		ZusatzDate10,
		ZusatzVarchar01,
		ZusatzVarchar02,
		ZusatzVarchar03,
		ZusatzVarchar04,
		ZusatzVarchar05,
		ZusatzVarchar06,
		ZusatzVarchar07,
		ZusatzVarchar08,
		ZusatzVarchar09,
		ZusatzVarchar10
  FROM SablonPozAruk


----------------------------------------------------------------------------------------------------------------
-- Schlussel	-> SysTbl_Keys
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		Code1,
		Code2
  FROM Schlussel


----------------------------------------------------------------------------------------------------------------
-- SEQMSPED1	-> SysTbl_Texts
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		Sprache								Lang,
		Art									[Type],
		Nummer								Number,
		[Text]								[Text],
		LetzteAenderung						LastModified_Date,
		Bemerkungen							Descr1,
		StatusText							Descr2,
		TipText								Descr3
  FROM SEQMSPED1	SysTbl_Texts


----------------------------------------------------------------------------------------------------------------
-- Statif	-> Status_Log
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		RecordID,
		StatusID,
		Varchar1,
		Varchar2,
		Varchar3,
		Long1,
		Long2,
		Long3,
		Datum1,
		Datum2,
		Datum3,
		UserID,
		Terminal,
		FelvetelDatum
  FROM Statif


----------------------------------------------------------------------------------------------------------------
-- Statuscodes	-> Status_Codes
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		KurzZeichen							Code,
		Beschreibung						Descr1,
		Beschreibung2						Descr2,
		Beschreibung3						Descr3,
		OrderLevel							Status_Level,
		Moveable,
		CanMoveFromPreOrder,
		CanMoveToPreOrder,
		MoveableBetweenOrders,
		BillIn,
		BillOut,
		OrderClose,
		RateToNewOrder,
		EnabledMoveToWarehose,
		StatusFlags,
		Hataskor,							-- KI LESZ VEVE AZ ADATBAZISBOL
		SzintVarchar,
		SzintLong,
		StatusFlags2,
		EnabledPreCalc,
		EnabledAfterCalc
  FROM Statuscodes


----------------------------------------------------------------------------------------------------------------
-- StatusCodes_After	-> Status_Codes_After
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		StatusID,
		After_StatusID,
		StoredProcedure,
		UserFld_int_01,
		UserFld_int_02,
		UserFld_int_03,
		UserFld_varchar_01,
		UserFld_varchar_02,
		UserFld_varchar_03
  FROM StatusCodes_After


----------------------------------------------------------------------------------------------------------------
-- StruktNr	-> SysTbl_StrucNum
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Typ,
		BordNr_1,
		BordNr_2,
		BordNr_3,
		BordNr_4,
		LastNr
  FROM StruktNr


----------------------------------------------------------------------------------------------------------------
-- SysSEL_PartIDs	-> SysTbl_PartIDs
----------------------------------------------------------------------------------------------------------------
SELECT PartID,
		Descr,
		Scope,
		PartTable,
		PartField
  FROM SysSEL_PartIDs


------------------------------------------------------------------------------------------------------
-- Szamlak	-> Inv
------------------------------------------------------------------------------------------------------

SELECT	ID									ID,
		TransactID							TransactID,
		SzlaSzam							Inv_Num,
		StornoSorszam						CancelInv_Num,
		LongReNr							Inv_Num_int,
		Iktatosorszam						Inv_SeqNum,
		Period								ACCT_Periods_ID,
		TipusID								TypeID,
		Ref_Szamlak_ID						Ref_Inv_ID,
		Parent_INV_ID						Parent_INV_ID,
		Allapot								Inv_Status,
		FormatID							FormatID,
		Gedruckt							Printed,
		StornoGedruckt						CancelPrinted,
		StornoDatum							CancelDate,
		AbschlussJN							ClosedYN,
		Szoveg								Note2,
		SzallitoKod							Vendor_ID,
		SzallitoNev1						Vendor_Name1,
		SzallitoNev2						Vendor_Name2,
		SzallitoOrszag						Vendor_Country,
		SzallitoState						Vendor_State,
		SzallitoISZ							Vendor_ZIP,
		Vendor_Addr_District				Vendor_Addr_District,
		SzallitoVaros						Vendor_City,
		SzallitoUtca						Vendor_Addr,
		Vendor_Addr_ps_type					Vendor_Addr_ps_type,
		Vendor_Addr_housenr					Vendor_Addr_housenr,
		Vendor_Addr_building				Vendor_Addr_building,
		Vendor_Addr_stairway				Vendor_Addr_stairway,
		Vendor_Addr_floor					Vendor_Addr_floor,
		Vendor_Addr_door					Vendor_Addr_door,
		SzallitoAdoszam						Vendor_TaxNum,
		SzallitoEURAdoszam					Vendor_TaxNum2,
		SzallitoPenzforgJelz				Vendor_BankCode,
		SzallitoSzlaszam					Vendor_BankAcc,
		SzallitoIBANCode					Vendor_IBAN,
		FIBUKontoKred						Vendor_AccNum_ID,
		VevoKod								Cust_ID,
		VevoNev1							Cust_Name1,
		VevoNev2							Cust_Name2,
		VevoOrszag							Cust_Country,
		VevoState							Cust_State,
		VevoISZ								Cust_ZIP,
		Customer_Addr_District				Customer_Addr_District,
		VevoVaros							Cust_City,
		VevoUtca							Cust_Addr,
		Customer_Addr_ps_type				Customer_Addr_ps_type,
		Customer_Addr_housenr				Customer_Addr_housenr,
		Customer_Addr_building				Customer_Addr_building,
		Customer_Addr_stairway				Customer_Addr_stairway,
		Customer_Addr_floor					Customer_Addr_floor,
		Customer_Addr_door					Customer_Addr_door,
		VevoAdoszam							Cust_TaxNum,
		VevoEURAdoszam						Cust_TaxNum2,
		VevoPenzforgJelz					Cust_BankCode,
		VevoSzlaszam						Cust_BankAcc,
		VevoIBANCode						Cust_IBAN,
		FIBUKontoDeb						Cust_AccNum_ID,
		BankkontoID							Bank_AC_ID,
		ClassID								ClassID,		-- (??? - mi ez?)
		Period_FROM							Period_FROM,
		Period_TO							Period_TO,
		Kelte								InvDate,
		Teljesitve							DeliveryDate,
		Curr_Dates_Method					Curr_Dates_Method,
		ArfDatum							CurrDate,
		KonyvelesiDatum						AccDate,
		FizMod								PaymentMethod,
		Lejarat								DueDate,
		BeerkezesDatum						PostInDate,
		BejovoSzlaLejarat					InvInDueDate,
		NettoOsszesen						Netto_LC,
		AFAOsszesen							Tax_LC,
		BruttoOsszesen						Brutto_LC,
		FizAllapot							PayStatus,
		Fully_paid_date						Fully_paid_date,
		EddigKifizetve						PaidAmount_DC,
		EddigKifizetveEUR					PaidAmount_FC,
		EddigKifizetveFIBU					PaidAmount_LC,
		FWGesamtNetto						Netto_DC,		-- DC = Document currency
		FWGesamtMwSt						Tax_DC,
		FWGesamtBrutto						Brutto_DC,
		SzamlaNyelve						Lang,
		Wahrung								Curr_ID,
		EURNetto							Netto_FC,
		EURMwSt								Tax_FC,
		EURBrutto							Brutto_FC,
		Bemerkung							Remarks,
		Mellekletek							Attachments,
		FelvDatum							Added_Date,
		FelvUserID							Added_Users_ID,
		SzlaFeladInfo						ACC_Info,
		--SAP_DocEntry						Nem kell,
		Subcontracted_Services				Subcontracted_Services,
		LgrBew_ID							Wrhs_Tran_ID,
		Current_Valid_Correction_Note_ID	Current_Valid_Correction_Note_ID,
		Cancellation_ReasonCode				Cancellation_ReasonCode,
		Cancellation_ReasonText				Cancellation_ReasonText,
		ZusatzInt01							UserFld_int01,
		ZusatzInt02							UserFld_int02,
		ZusatzInt03							UserFld_int03,
		ZusatzFloat01						UserFld_float01,
		ZusatzFloat02						UserFld_float02,
		ZusatzFloat03						UserFld_float03,
		ZusatzVarchar01						UserFld_nvarchar01,
		ZusatzVarchar02						UserFld_nvarchar02,
		ZusatzVarchar03						UserFld_nvarchar03,
		ZusatzDate01						UserFld_date01,
		ZusatzDate02						UserFld_date02,
		ZusatzDate03						UserFld_date03
FROM	Szamlak		Inv


------------------------------------------------------------------------------------------------------
-- Szamlatetelek	-> Inv_L
------------------------------------------------------------------------------------------------------

SELECT	ID									ID,
		TransactID							TransactID,
		SzamlaID							Inv_ID,
		Alsorszam							SeqNum,
		Ref_Komp_Szamlak_ID					Comp_Inv_ID,
		--RefFrom_Szamlak_ID				Nem kell,
		--RefFrom_Szamlatetelek_ID			Nem kell,
		--RefTo_Szamlatetelek_ID			Nem kell,
		--PosTipusID						Nem kell,
		PosID								Pos_ID,
		PozSzam								Ord_Num,
		ResztvevoID							Part_ID,
		--ResztvevoCode						Nem kell,
		TarifID								Rates_ID,
		Megnevezes							Descr,
		TarifBemerkung						Note,
		Darab								Pcs,
		ME									Unit,
		Egysegar							UnitPrice_DC,
		Netto								Netto_DC,
		SteuerCode							ACCT_TaxCodes_ID,
		AFAkulcs							TaxRate,
		AFA									Tax_DC,
		Brutto								Brutto_DC,
		FWStkPreis							UnitPrice_FC2,
		FWNetto								Netto_FC2,
		FWMwSt								Tax_FC2,
		FWBrutto							Brutto_FC2,
		Wahrung								Curr_ID,
		Kurs								Rate_DC,
		KursEinheit							Rate_Unit_DC,
		KursDatum							Rate_Date_DC,
		EURKurs								Rate_FC,
		EURKursEinheit						Rate_Unit_FC,
		EURStkPreis							UnitPrice_FC,
		EURNetto							Netto_FC,
		EURMwSt								Tax_FC,
		EURBrutto							Brutto_FC,
		EURKursDatum						Rate_Date_FC,
		OrigLeistID							Ord_Calc_ID,
		FIBUKurs							Rate_LC,
		FIBUKursEinheit						Rate_Unit_LC,
		FIBUKursDatum						Rate_Date_LC,
		FIBUStkPreis						UnitPrice_LC,
		FIBUNetto							Netto_LC,
		FIBUMwSt							Tax_LC,
		FIBUBrutto							Brutto_LC,
		--Calculated						Nem kell,
		--SAP_DocEntry						Nem kell,
		--SAP_LineNum						Nem kell,
		Period_FROM							Period_FROM,
		Period_TO							Period_TO,
		ZusatzInt01							UserFld_int01,
		ZusatzInt02							UserFld_int02,
		ZusatzInt03							UserFld_int03,
		ZusatzFloat01						UserFld_float01,
		ZusatzFloat02						UserFld_float02,
		ZusatzFloat03						UserFld_float03,
		ZusatzVarchar01						UserFld_nvarchar01,
		ZusatzVarchar02						UserFld_nvarchar02,
		ZusatzVarchar03						UserFld_nvarchar03,
		ZusatzDate01						UserFld_date01,
		ZusatzDate02						UserFld_date02,
		ZusatzDate03						UserFld_date03
  FROM		Szamlatetelek	Inv_L



----------------------------------------------------------------------------------------------------------------
-- Szamlatetelek_FIBU	-> INV_L_ACCT
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		ParentRecordID						ParentRecordID,
		SeqNum								SeqNum,
		KontoID								GLNums_ID,
		Fix									Amount,
		FixPercent							Perc,
		BelongTo_ID							BelongsTo_ID,
		Param1								Param1,
		Param2								Param2,
		Param3								Param3
FROM	Szamlatetelek_FIBU


----------------------------------------------------------------------------------------------------------------
-- Szemelyek	-> Cust_Contacts
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Cegek_ID							Cust_ID,
		Nev									Name1,
		NameInFremdsprache1					Name2,
		NameInFremdsprache2					Name3,
		Anrede								Title,
		Beosztas							Cust_Positions_ID,
		BeosztasPontosMegn					Cust_Positions_Name,
		MhTel1								Business_Phone1,
		MhTel2								Business_Phone2,
		MhFax								Business_Fax,
		Mobil								Business_Mobile,
		EMail								Business_Email,
		OtthonOrszag						Other_Country,
		OtthonISZ							Other_ZIP,
		OtthonVaros							Other_City,
		OtthonUtca							Other_Addr,
		OtthonTel1							Other_Tel1,
		OtthonTel2							Other_Tel2,
		Megjegyzesek						Notes
 FROM	Szemelyek	Cust_Contacts


----------------------------------------------------------------------------------------------------------------
-- Tarifen	-> Rates
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		KEZI								ShortName,
		ZeichenNummer						ActCode,
		MwStCode							Acct_TaxCodes_ID,
		Scope								Scope,
		Art									[Type],
		AlapDb								Qty,
		Egysegar							UnitPrice,
		Dev									Curr_ID,
		RateLevel							RateLeve,
		ExFile								ExFile,
		ExSheet								ExSheet,
		ExEbene								ExLevel,
		ExAnzKooNr							ExQtyCellNum,
		ExAnzKooStr							ExQtyCellStr,
		ExStkPreisKooNr						ExUnitPriceCellNum,
		ExStkPreisKooStr					ExUnitPriceCellStr,
		ExTextKooNr							ExNoteCellNum,
		ExTextKooStr						ExNoteCellStr,
		FN_Pcs								FN_Pcs,
		FN_UnitPrice						FN_UnitPrice,
		CostType							CostType,
		ZusatzInt01							UserFld_int01,
		ZusatzInt02							UserFld_int02,
		ZusatzInt03							UserFld_int03,
		ZusatzFloat01						UserFld_float01,
		ZusatzFloat02						UserFld_float02,
		ZusatzFloat03						UserFld_float03,
		ZusatzVarchar01						UserFld_nvarchar01,
		ZusatzVarchar02						UserFld_nvarchar02,
		ZusatzVarchar03						UserFld_nvarchar03
  FROM Tarifen


----------------------------------------------------------------------------------------------------------------
-- Tarifen_FIBU	-> Rates_Acct
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		ParentRecordID						ParentRecordID,
		FIBUDimensionen_ID					Acct_Views_ID,
		ParentKonto_ID						Parent_GLNums_ID,
		--ParentKontonummer,
		KontoID								GLNums_ID,
		--Kontonummer,
		Fix									Amount,
		FixPercent							Perc,
		BelongTo_ID							BelongsTo_ID,
		--BelongTo_Name,
		Param1								Param1,
		Param2								Param2,
		Param3								Param3
  FROM Tarifen_FIBU


----------------------------------------------------------------------------------------------------------------
-- Tarifen_Kif	-> SysTbl_Rates_Temp
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		Terminal,
		CalcBelongsTo_ID,
		ParametersFrom_ID,
		NewRecordID,
		Alsorszam,
		PosID,
		PozSzam,
		ResztvevoID,
		KiadasBevetel,
		FZID,
		FZName1,
		Sprache,
		TarifID,
		Megnevezes,
		TarifBemerkung,
		Inv_PerformanceDate,
		Inv_CurrencyDate,
		Darab,
		ME,
		BizDevEgysegar,
		BizDevNetto,
		AFAkod,
		AFAkulcs,
		BizDevAFA,
		BizDevBrutto,
		FWStkPreis,
		FWNetto,
		FWMwSt,
		FWBrutto,
		Wahrung,
		Kurs,
		KursEinheit,
		KursDatum,
		EURKurs,
		EURKursEinheit,
		EURStkPreis,
		EURNetto,
		EURMwSt,
		EURBrutto,
		EURKursDatum,
		FIBUKurs,
		FIBUKursEinheit,
		FIBUKursDatum,
		FIBUStkPreis,
		FIBUNetto,
		FIBUMwSt,
		FIBUBrutto,
		WithoutDiscount
  FROM Tarifen_Kif


----------------------------------------------------------------------------------------------------------------
-- TarifGruppen	-> Rates_Elements
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		KopfTarifID,
		TarifID,
		Alsorszam
  FROM TarifGruppen


----------------------------------------------------------------------------------------------------------------
-- TarifParams	-> Rates_Excel_Params
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		TarifenID,
		Alsorszam,
		ExKooNr,
		ExkooStr,
		ExWert
  FROM TarifParams


----------------------------------------------------------------------------------------------------------------
-- Terminals	-> SysTbl_Terminals
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		TerminalName,
		UserID,
		EinLogTime,
		[Password]
  FROM Terminals


----------------------------------------------------------------------------------------------------------------
-- Transact_errors	-> SysTbl_Trans_Err
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		ErrTime,
		ProcedureName,
		RecordID,
		ErrorCode,
		ErrorText,
		Terminal,
		TableName,
		FieldName,
		ForUser
  FROM Transact_errors


----------------------------------------------------------------------------------------------------------------
-- Transacts	-> SysTbl_Trans
----------------------------------------------------------------------------------------------------------------
SELECT TransactID,
		TransactDate,
		Terminal,
		TableName,
		FieldName,
		ForUser,
		UserID,
		ProcedureName,
		RecordID,
		ParentTransactID,
		NoTriggers,
		SavePoint
  FROM Transacts


----------------------------------------------------------------------------------------------------------------
-- TS_Kopf	-> RateSrv
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		StoreDate,
		[Status]
  FROM TS_Kopf


----------------------------------------------------------------------------------------------------------------
-- TS_Parameters	-> RateSrv_L_Params
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		TS_SatzeID,
		Alsorszam,
		ExKooNr,
		ExKooStr,
		ExWert,
		ExWertCalculated
  FROM TS_Parameters


----------------------------------------------------------------------------------------------------------------
-- TS_Satze	-> RateSrv_L
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		KopfID,
		CalcBelongsTo_ID,
		RefID,
		ExEbene,
		ParametersFrom_ID,
		ExFile,
		ExSheet,
		ExAnzKooNr,
		ExAnzKooStr,
		ExStkPreisKooNr,
		ExStkPreisKooStr,
		ExTextKooNr,
		ExTextKooStr,
		ErgAnz,
		ErgStkPreis,
		ErgText,
		ErrorCode,
		ErrorParam
  FROM TS_Satze


----------------------------------------------------------------------------------------------------------------
-- UserGruppen	-> Usr_Grp
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Benennung
  FROM UserGruppen


----------------------------------------------------------------------------------------------------------------
-- Users	-> Usr
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		UserGruppe,
		KurzName,
		Passwort,
		Stufe,
		Sprache,
		Name,
		Zimmer,
		Durchwahl,
		Titel,
		Sonstiges,
		MCFremdsystem1,
		MCFremdsystem2,
		Alkalmazottak_ID
  FROM Users


----------------------------------------------------------------------------------------------------------------
-- UserStufen	-> Usr_Levels
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Stufe,
		SendungenModifizieren,
		SendungenModifizierenNachAbschluss,
		SablonModifizieren,
		AlleSendungenMod,
		ReDrucken,
		ReModifizieren,
		SpedibuchDrucken,
		EingangsReModifizieren,
		KundenstammErfassen,
		KundenstammModifizieren,
		KundenstammLoschen,
		KundenStammDrucken,
		KundenStammKreditLimitMod,
		ArtikelstammBearbeiten,
		PersonalStammBearbeiten,
		LKWStammBearbeiten,
		PersonalStammAlleDaten,
		ParaStammBearbeiten,
		WahrungStammBearbeiten,
		TarifenBearbeiten,
		EntfernungenBearbeiten,
		WahrungGrunddatenAndern,
		BankkontoNrBearbeiten,
		FixTexteBearbeiten,
		KonfigAendern,
		FIBUKontenEditieren,
		BankBearbeiten,
		KasseBearbeiten,
		ChqBearbeiten,
		RechnungDokuStandBis,
		KasseDokuStandBis,
		BankDokuStandBis,
		ChqDokuStandBis,
		TurenBearbeiten,
		TurenBearbeitenNachAbschluss,
		LgrGrundDatenBearbeiten,
		LgrBewegungenBearbeiten,
		ExcelExport,
		WorkInClosedPeriod,
		TransactionsBetweenPeriods,
		CalcModifyAfterBilling,
		UserSpec01,
		UserSpec02,
		UserSpec03,
		UserSpec04,
		UserSpec05,
		UserSpec06
  FROM UserStufen


----------------------------------------------------------------------------------------------------------------
-- Version	-> Vers
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		VersionNr,
		VersionDate
  FROM [Version]


----------------------------------------------------------------------------------------------------------------
-- VersionDetails	-> Vers_Details
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		Alsorszam,
		VersionID,
		StepID,
		[Description],
		Script
  FROM VersionDetails


----------------------------------------------------------------------------------------------------------------
-- Wahrungen	-> Curr
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		KurzName							Shortname,
		KurzZeichen							Internal_Code,
		Benennung							Descr,
		KursPro								Rates_Unit,
		MCFremdsystem1						LinkCode_1,
		MCFremdsystem2						LinkCode_2,
		IntRundung							Rounding,
		NotInUse							NotInUse
FROM	Wahrungen


----------------------------------------------------------------------------------------------------------------
-- ZIPCodes	-> ZIPCodes
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		CountryCode,
		ZIPFrom,
		ZIPTo,
		Place,
		Zone
  FROM ZIPCodes


----------------------------------------------------------------------------------------------------------------
-- ZMENUE	-> ParmTbl_Menues
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		TransactID							TransactID,
		UserID,
		UserGruppe,
		Sprache,
		MenuNr,
		Spalte,
		Reihe,
		Titel,
		Startformular,
		Unsichtbar,
		NichtErreichbar,
		Param1
  FROM ZMENUE


----------------------------------------------------------------------------------------------------------------
-- ZMENUE0	-> SysTbl_Menues
----------------------------------------------------------------------------------------------------------------
SELECT	ID									ID,
		UserID,
		UserGruppe,
		Sprache,
		MenuNr,
		Spalte,
		Reihe,
		Titel,
		Startformular,
		Unsichtbar,
		NichtErreichbar,
		Param1
  FROM ZMENUE0


