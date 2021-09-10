Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

'Compiler parameters:
'SEL_EXTERNAL    Ha "1", akkor external modul, kulonben a SELEXPED program maga
'SEL_PLATFORM    Megadja a fejlesztoi platform-ot.
'                Lehetseges ertekek: "WIN_OS"
'                                    "WIN_MOBILE"
'                                    "WEB"

Imports System.IO

#Region "Enumerators"

Public Enum ENUM_AddressClass As Integer
    NOT_DEFINED = 0
    Customer = 1
    Carrier = 2
    Terminal = 3
    Custom = 4
    Agent = 5
End Enum

Public Enum ENUM_Address_Data_Format As Integer
    NORMAL = 0
    SIMPLE = 1
End Enum

Public Enum ENUM_AddressTypes As Integer
    NONE = 0
    Ord_E = 10
    Ord_Sub1 = 14
    Ord_Sub2 = 15
    Ord_Mcarr = 16
    Ord_CC1 = 17
    Ord_CC2 = 19
    Ord_L_E = 20
    Ord_L_LoadingPlace = 21
    Ord_L_UnloadingPlace = 22
    Ord_L_Carr = 23
    Ord_L_Sub1L = 24
    Ord_L_Sub2L = 25
    Ord_L_Cust = 26
    Ord_L_Sub3 = 27
    Ord_L_CC_M = 28
    Ord_L_McarrL = 29
    Goods_Ord_E = 30
    Goods_Sub1 = 34
    Goods_Sub2 = 35
    Goods_Carr_M = 36
    Goods_Cust_M = 39
    Goods_L_E = 40
    Goods_Shppr = 41
    Goods_Cnsg = 42
    Goods_Carr = 43
    Goods_Sub1L = 44
    Goods_Sub2L = 45
    Goods_Cust = 46
    Goods_Sub3 = 47
    Goods_Cust_L = 48
    Goods_Carr_L = 49
    Goods_RouDr1 = 101
    Goods_RouDr2 = 102
    WRHS_O = 301
    WRHS_I = 302
    WRHS_Carrier_IN = 303
    WRHS_Carrier_OUT = 304
    Airline = 400
    AirPort1 = 401
    AirPort2 = 402
    AirPort3 = 403
    AirPort4 = 404
    AirPort5 = 405
    Carr = 1001
    OfferedTO = 1002
    ShippingLine = 1003
    MBL_Shipper = 1006
    MBL_Consignee = 1007
    MBL_Notify = 1008
    MBL_AlsoNotify = 1015
    HBL_Shipper = 1009
    HBL_Consignee = 1010
    HBL_Notify = 1011
    HBL_AlsoNotify = 1014
    PickUp = 1016
    PickUp_Term = 1017
    PickUp_Carr = 1018
    PickUp_DepTerm = 1019
    PickUp_DepTerm_Carr = 1020
    PickUp_ArrTerm = 1021
    PickUp_ArrTerm_Carr = 1022
    PoRec = 1012
    PoRec_Term = 1023
    PoRec_Carr = 1024
    PoRec_Cust = 1025
    ExpCust = 1026
    ExpCust_Term = 1027
    ExpCust_Carr = 1028
    ExpTerm = 1029
    ExpTerm_Carr = 1030
    ExpTerm_Cust = 1031
    PoL = 1004
    PoL_Agent = 1049
    PoL_Term = 1032
    PoL_Carr = 1039
    PoL_Cust = 1033
    TransShipmentPort1 = 1034
    TransShipmentPort2 = 1035
    TransShipmentPort3 = 1036
    TransShipmentPort4 = 1037
    TransShipmentPort5 = 1038
    PoD = 1005
    PoD_Agent = 1050
    PoD_Term = 1040
    PoD_Cust = 1041
    PoD_Carr = 1042
    PoDel = 1013
    PoDel_Cust = 1057
    ImpTerm = 1043
    ImpTerm_Carr = 1044
    ImpTerm_Cust = 1045
    ImpCust = 1046
    ImpCust_Term = 1047
    ImpCust_Carr = 1048
    DropOff_Carr = 1059
    DropOff_DepTerm = 1051
    DropOff_DepTerm_Carr = 1052
    DropOff_ArrTerm = 1053
    DropOff_ArrTerm_Carr = 1058
    DropOff = 1054

    PoDTerm_Carr = 1061
    PoLTerm = 1062
    PoDTerm = 1063
    PoLCust = 1064
    PoDCust = 1065
    Origin = 1066
    Insurance = 1067
    GenAgentDepo = 1068
    GenAgent = 1069
    GenAgentCarr = 1070
    HBL_Cust = 1071

    FTL_L_Carrier = 3001

    CONT_OfferedTO = 5002
    CONT_ShippingLine = 5003
    CONT_MBL_Shipper = 5006
    CONT_MBL_Consignee = 5007
    CONT_MBL_Notify = 5008
    CONT_MBL_AlsoNotify = 5015
    CONT_HBL_Shipper = 5009
    CONT_HBL_Consignee = 5050
    CONT_HBL_Notify = 5011
    CONT_HBL_AlsoNotify = 5014
    CONT_PickUp = 5016
    CONT_PickUp_Term = 5017
    CONT_PickUp_Carr = 5018
    CONT_PickUp_DepTerm = 5019
    CONT_PickUp_DepTerm_Carr = 5020
    CONT_PickUp_ArrTerm = 5021
    CONT_PickUp_ArrTerm_Carr = 5022
    CONT_PoRec = 5012
    CONT_PoRec_Term = 5023
    CONT_PoRec_Carr = 5024
    CONT_PoRec_Cust = 5025
    CONT_ExpCust = 5026
    CONT_ExpCust_Term = 5027
    CONT_ExpCust_Carr = 5028
    CONT_ExpTerm = 5029
    CONT_ExpTerm_Carr = 5030
    CONT_ExpTerm_Cust = 5031
    CONT_PoL = 5004
    CONT_PoL_Agent = 5049
    CONT_PoL_Term = 5032
    CONT_PoL_Carr = 5039
    CONT_PoL_Cust = 5033
    CONT_TransShipmentPort1 = 5034
    CONT_TransShipmentPort2 = 5035
    CONT_TransShipmentPort3 = 5036
    CONT_TransShipmentPort4 = 5037
    CONT_TransShipmentPort5 = 5038
    CONT_PoD = 5005
    CONT_PoD_Agent = 5050
    CONT_PoD_Term = 5040
    CONT_PoD_Cust = 5041
    CONT_PoD_Carr = 5042
    CONT_PoDel = 5013
    CONT_PoDel_Cust = 5057
    CONT_ImpTerm = 5043
    CONT_ImpTerm_Carr = 5044
    CONT_ImpTerm_Cust = 5045
    CONT_ImpCust = 5046
    CONT_ImpCust_Term = 5047
    CONT_ImpCust_Carr = 5048
    CONT_DropOff_Carr = 5059
    CONT_DropOff_DepTerm = 5051
    CONT_DropOff_DepTerm_Carr = 5052
    CONT_DropOff_ArrTerm = 5053
    CONT_DropOff_ArrTerm_Carr = 5058
    CONT_DropOff = 5054

    CONT_PoDTerm_Carr = 5061
    CONT_PoLTerm = 5062
    CONT_PoDTerm = 5063
    CONT_PoLCust = 5064
    CONT_PoDCust = 5065
    CONT_Origin = 5066
    CONT_Insurance = 5067
    CONT_GenAgentDepo = 5068
    CONT_GenAgent = 5069
    CONT_GenAgentCarr = 5070
    CONT_HBL_Cust = 5071
End Enum

Public Enum ENUM_Arf_Codes As Integer
    NONE = 0
    ARF_CSAK_SZORZAS = 1
    ARF_UJRA = 2
End Enum
Public Enum ENUM_BANKART As Integer
    BANK_AUS_EINRE = 1 'uberweisung Eingangsrechnungbetrag
    BANK_EIN_AUSRE = 2 'Ankommen Ausgangsrechnung
    BANK_AUS_BANK_KASSE = 3
    BANK_EIN_KASSE_BANK = 4
    BANK_AUS_SONST = 5
    BANK_EIN_SONST = 6
    BANK_AUS_KOMP = 7
    BANK_EIN_KOMP = 8
End Enum
Public Enum ENUM_Casual_DeliveryDate_Code As Integer
    Same_As_Casual = 0
    PeriodBegin = 1
    PeriodEnd = 2
    Deadline = 3
    InvoiceDate = 4
    Now = 5
End Enum
Public Enum ENUM_COUNTRY_TYPE As Integer
    NOT_MEMBER = 0
    EU_MEMBER = 1
End Enum
Public Enum ENUM_CrDr As Integer
    NOT_DEFINED = 0
    MINUS = 1
    PLUS = 2
    INTERNAL_MINUS = 4
    INTERNAL_PLUS = 3
End Enum
Public Enum ENUM_CreditNoteTypes As Integer
    Kimeno_jovairas_felvetele = 203
    Bejovo_jovairas_felvetele = 205
    Kimeno_strono_jovairas_felvetele = 206
    Bejovo_helyesbito_szamla_felvetele = 207
    Kimeno_helyesbito_szamla_felvetele = 208
End Enum
Public Enum ENUM_Cust_AddressTypes As Integer
    NORMAL = 0
    MAIL = 1
    SITE = 2
    INV = 3
    OTHER_CUST = 4
End Enum

Public Enum ENUM_Cust_TaxNum_Control_Method As Integer
    NORMAL = 0
    NO_CHECK = 9
End Enum

Public Enum ENUM_DELIVERYDATE_CODE
    NOT_DEFINED = 0
    MAX_DELIVERY_DATE = 1
    MIN_DELIVERY_DATE = 2
    INV_DATE = 3
    CURRENT_DATE = 4
    INV_DEADLINE = 5
End Enum
Public Enum ENUM_DELIVERYDATE_CODE_L
    NOT_DEFINED = 0
    UNLOADING = 1
    LOADING = 2
    INV_DATE = 3
    CURRENT_DATE = 4
    INV_DEADLINE = 5
    EXPIMP = 6
    INV_DELIVERYDATE = 7
End Enum
Public Enum ENUM_Field_Params
    NOT_DEFINED = 0
    VISIBLE = 1
    MANDATORY = 2
    LOCKED = 3
    XTYPE_VB = 4
    F_FORMAT = 5
    F_FORMAT_NOSPACE = 6
    F_FORMAT_TRIM = 7
    F_FORMAT_UCASE = 8
    F_FORMAT_NoShow0 = 9
    F_FORMAT_MinusAllowed = 10
    F_FORMAT_LABEL_ALIGN = 11
    F_FORMAT_FORMAT = 12
    F_FORMAT_ALIGN = 13

    COLOR_LABEL_BG = 14
    COLOR_LABEL_FORE = 15
    COLOR_NORMAL_BG = 16
    COLOR_NORMAL_FORE = 17
    COLOR_SELECTED_FORE = 18

    BG_IMAGE = 19
    TAG = 20
    TABSTOP = 21
    DT_FIXTEXT_KEY = 22
    DT_WHERE2 = 23
    BASE_VALUE = 24
End Enum
Public Enum ENUM_INV_DIRECT_METHOD As Integer
    INV = 0
    INV_AND_CREDITNOTE = 1
End Enum

Public Enum ENUM_INV_SUBCONTRACTED_SERVICES_SHOW
    In_HEAD = 0
    In_LINE = 1
End Enum
Public Enum ENUM_INV_DELIVERYDATE_HEAD_METHOD
    NOT_DEFINED = 0
    LineDelivery_Max = 1
    LineDelivery_Min = 2
    Inv_Date = 3
    Inv_DueDate = 4
End Enum
Public Enum ENUM_INV_PLANED_PAYMENT_METHOD
    FROM_DUEDATE = 0
    FROM_POSTINDATE = 1
    FROM_CUST_CREDIT_DUEDAYS = 2
End Enum
Public Enum ENUM_INV_Inv_And_Calc_Manage_Type
    NORMAL = 0
    INV_AND_CALC_ARE_SAME = 1
End Enum
Public Enum ENUM_INV_Line_Handling_Type
    NORMAL = 0
    INV_LINE_ONLY_FROM_CALC = 1
End Enum
Public Enum ENUM_INV_CreditNote_Sign_Manage
    PCS_is_Negative = 0
    UNITPRICE_is_Negative = 1
End Enum
Public Enum ENUM_INV_Calc_Sign_Manage
    NORMAL = 0 'A szamlakhoz es jovairasokhoz minden kalkulacio hozzaadhato
    ONLY_WITH_SAME_SIGN = 1 'Szamlakhoz csak a pozitiv netto osszegu kalkulaciok adhatok hozza,
    '                        jovairasokhoz csak a negativ netto osszeguek.
End Enum
Public Enum ENUM_INV_Save_Origin_To_PDF
    Default_Save_With_Question = 0
    No_Save = 1
    Save_With_Question = 2
    Save_Auto_To_Map_Monthly = 3
    Save_Auto_To_Map_Yearly = 4
End Enum

Public Enum ENUM_DEADLINE_TYPES
    CALENDAR_DAYS = 0
    BANK_DAYS = 1
    LAST_DAY_OF_MONTH_THAN_CALENDAR_DAYS = 101
    CALENDAR_DAYS_THAN_LAST_DAY_OF_MONTH = 102
End Enum

Public Enum ENUM_PERIODIC_DELIVERYDATE As Integer
    AS_BY_CASUAL_INV = 0
    PERIOD_END = 1
    PERIOD_BEGIN = 2
    INV_DATE = 3
    CURRENT_DATE = 4
    INV_DEADLINE = 5
End Enum
Public Enum ENUM_Direction As Integer
    None = -1
    Right = 0
    Top = 90
    Left = 180
    Bottom = 270
End Enum
Public Enum ENUM_DOCMAN_SaveStyle As Integer
    ToFile = 0
    ToDB = 1
End Enum
Public Enum ENUM_ERRDIAL_TYPE As Long
    DIALNUM_STANDARD = 0
    DIALNUM_RETVALUE = -1
    NODIALOG = -2
End Enum
Public Enum ENUM_EXP_IMP
    Inland = 0
    Export = 1
    Import = 2
    Foreign = 3
End Enum
Public Enum ENUM_FP_CONTROL_Created_by As Integer
    User = 0
    RS = 1
    GRID = 2
End Enum
Public Enum ENUM_FP_CONTROL_REFRESH_TYPE As Integer
    Normal = 0
    On_Form_Current = 1
    On_Form_AfterUpdate = 2
End Enum
Public Enum ENUM_FUNCTIONLOCK_IDs
    ALL = 0
    WEB_EXCHANGE_RATES = 1
End Enum
Public Enum ENUM_LAYOUT_STYLE As Integer
    NORMAL = 0
    TABLET = 1
End Enum

Public Enum ENUM_INV_CLASS As Integer
    NOT_DEFINED = 0
    BY_CASE = 1
    PERIODIC = 2
    PERIODIC_WEEK = 3
    PERIODIC_MONTH = 4
    PERIODIC_2_MONTHS = 5
    PERIODIC_QUARTAL = 6
    PERIODIC_HALF_YEAR = 7
    PERIODIC_YEAR = 8
End Enum
Public Enum ENUM_INV_PARAMS_ROUNDING As Integer
    PER_INV_LINE = 0
    ONLY_AT_THE_END = 1
    PER_TAXCODE = 2
End Enum
Public Enum ENUM_INV_PARAMS_INV_FOR As Integer
    ALL = 0
    HAVE_ACCT_Cust_ID = 1
    HAVE_LINKCODE_1 = 2
    HAVE_LINKCODE_2 = 3
    HAVE_LINKCODE_1_FOR_OUT_AND_HAVE_INKCODE_2_FOR_IN = 4
End Enum
Public Enum ENUM_INV_PAYMENT_METHOD As Integer
    NOT_DEFINED = 0
    CASH = 1
    BANK_TRANSFER = 2
    CHECK = 3
End Enum
Public Enum ENUM_INV_Status As Integer
    NOT_DEFINED = 0
    IN_FOLDER = 4
    BY_ACCOUNTANT = 5
    ACCOUNTED = 6
End Enum
Public Enum ENUM_INV_Types
    NOT_DEFINED = 0
    OUT = 201
    [IN] = 202
    OUT_CREDITNOTE = 203
    ACC_DOC_IN = 204
    ACC_DOC_OUT = 213
    IN_CREDITNOTE = 205
    OUT_CREDITNOTE_CANCEL = 206
    IN_CORRECTION = 207
    OUT_CORRECTION = 208
    ADVANCE_PAYMENT = 209
    RETURNS = 210
    OUT_CORRECTION_DOC = 211
    IN_CORRECTION_DOC = 212
    DEPOSIT = 214
    CREDITNOTE_DEPOSIT = 215
End Enum
Public Enum ENUM_Menu_State As Integer
    Hided = 0
    Showed = 1
End Enum
Public Enum ENUM_PAYSTATE As Integer
    NOT_DEFINED = 0
    NOT_PAYED = 1
    IN_BANK_TRANSFER = 2
    PAYED = 3
    CANCELLED = 4
    CORRECTED = 5
    NOT_RECOVERABLE = 6
End Enum
Public Enum ENUM_PrintMode As Integer
    Preview = 1
    Direct = 2
End Enum
Public Enum ENUM_RecordStatus As Long
    EXISTS = 1
    NORECORD = 2
    NEWRECORD = 3
End Enum
Public Enum ENUM_ReportOpenType As Integer
    ToPrinter = 0
    ToScreen = 2
End Enum
Public Enum ENUM_ReportSource As Integer
    CurrentRecord = 0
    RS = 1
    RS_with_Select = 2
End Enum
Public Enum ENUM_ReportType As Integer
    RDLC = 0
    WORD = 1
    HTML = 2
    WORDX = 3
End Enum
Public Enum ENUM_SavePoint_Type As Integer
    NONE = 0
    ON_GOTFOCUS = 1
    TRANSACT_MODE = 2
End Enum
Public Enum ENUM_SCOPE
    SCOPE_NONE = 0
    SCOPE_ORD = 1
    SCOPE_ORD_L = 2
    SCOPE_ORD_CONT = 201
End Enum
Public Enum ENUM_ServerObject_Type As Integer
    None = 0
    P = 1 '= Stored Procedure
    V = 2 '= Table or View (should have instead of triggers)
    TF = 3 '= Table valued function - Only by Object READ And Object GRID
End Enum
Public Enum ENUM_Subcontracted_Services_Method
    Allways_TRUE = 0
    Allways_FALSE = 1
    True_When_Subcontracter_Exists = 2
End Enum
Public Enum ENUM_SQLOBJTYPES As Integer
    None = 0
    FN = 1
    P = 2
    TR = 3
    U = 4
    V = 5
End Enum
Public Enum ENUM_TableCodes As Integer
    UNKNOWN = 0
    ORD = 1
    ORD_L = 2
    ORD_C = 201
    ORD_GOODS = 5
    WRHS_L = 3
    TOURES = 4
End Enum
Public Enum ENUM_SELEXPED_Folder_Types As Integer
    TEMP = 0
    REPORT_TEMP = 1
    PREPARED_TEMPLATES = 2
End Enum
Public Enum ENUM_StrDate_Foramt As Integer
    FOR_SQL_SERVER = 0
    FOR_DOTNET = 1
End Enum
Public Enum ENUM_Taxpayer_Types As Integer
    LEGAL_ENTITY = 0 'Jogi személy
    INDIVIDUAL = 1 'Maganszemely
End Enum
Public Enum ENUM_UNITS As Integer
    METER = 1
    CENTIMETER = 100
    MILLIMETER = 1000
End Enum
Public Enum ENUM_UpDown As Long
    UP = 1
    DOWN = -1
End Enum
Public Enum ENUM_UPDATE_METHOD As Integer
    MANUALY = 0
    AUTOMATED = 1
End Enum
Public Enum ENUM_WEB_Curr_Manage As Integer
    NOT_DEFINED = 0
    ALLOWED = 1
    NOT_ALLOWED = 2
End Enum
Public Enum ENUM_DOCMAN As Integer
    DELETE_WITH_CONFIRM = 0
    DELETE_WITHOUT_CONFIRM = 1
    NO_DELETE = 2
End Enum
Public Enum ENUM_CREDIT_LIMIT_CHK_MODE
    NO_CHECK = 0
    INCOME_ONLY = 1
    CHECK_ALL = 2
End Enum
Public Enum ENUM_Ecomm_Send_Via As Integer
    None = 0
    Via_Outlook = 1
    Via_SMTP = 2
    Via_MAILTO = 3
End Enum

#End Region
#Region "Structures"
Public Structure Struct_ACCT_Periods_Info
    Dim ACCT_Periods_ID As Long
    Dim ACCT_Periods_Name As String
    Dim Parent_ACCT_Periods_ID As Long
    Dim Parent_ACCT_Periods_Name As String
    Dim Date_FROM As DateTime
    Dim Date_TO As DateTime
    Dim No_New_Order As Boolean
    Dim CSHBK_Closed As Boolean
    Dim INV_Outgoing_Closed As Boolean
    Dim INV_Incoming_Closed As Boolean
End Structure
Public Structure STRUCT_COMBOBOX_PARAMS
    Dim c As ComboBox
    Dim SQL_SELECT_0 As String
    Dim SQL_SELECT As String
    Dim SQL_FROM As String
    Dim SQL_WHERE As String
    Dim SQL_WHERE_FOR_LIST As String
    Dim SQL_GROUPBY As String
    Dim SQL_ORDERBY As String
    Dim DT As DataTable
    Dim ValueMember As String
    Dim DisplayMember As String
End Structure
Public Structure Struct_CONTROL_PROPS
    Dim Type As String
    Dim Name As String
    Dim ClientRectangle As Rectangle
    Dim Parent As String
    Dim Label_Name As String
    Dim Label_Clientrechtangle As Rectangle
End Structure
Public Structure Struct_DATA_Field_Props
    Dim SeqNum As Long
    Dim xtype As SqlDbType
    Dim xtype_VB As String
    Dim xLength As Integer
    Dim Value As String
End Structure
Public Structure Struct_DeliveryDates
    Dim AddressTypes_ID As ENUM_AddressTypes
    Dim CrDr As ENUM_CrDr
    Dim CUST_ID As Integer
    Dim DeliveryDate As DateTime
    Dim CurrDate As DateTime
End Structure
Public Structure Struct_DeliveryDates_CUST_INV_PARAMS
    Dim OUT_ClassID As ENUM_INV_CLASS
    Dim IN_ClassID As ENUM_INV_CLASS
    Dim OUT_CurrDate_Code As ENUM_DELIVERYDATE_CODE
    Dim IN_CurrDate_Code As ENUM_DELIVERYDATE_CODE
    Dim OUT_DeliveryDate_Code As ENUM_DELIVERYDATE_CODE
    Dim IN_DeliveryDate_Code As ENUM_DELIVERYDATE_CODE
End Structure
Public Structure Struct_DeliveryDates_Pos
    Dim Cimzett_Kulfoldi As Boolean 'vagyis export
    Dim ORD_AddedDate As DateTime
    Dim ORD_LoadingDate As DateTime
    Dim ORD_UnloadingDate As DateTime
    Dim ORD_L_AddedDate As DateTime
    Dim ORD_L_LoadingDate As DateTime
    Dim ORD_L_UnloadingDate As DateTime
End Structure
Public Structure Struct_DoFilter_gl_Params
    Dim DoIt As Boolean
    Dim ProcessID As Long
    Dim LetNewRecord As Boolean
    Dim FilterText As String
    Dim FilterWHERE As String
    Dim ParamInt() As Long
    Dim ParamDbl() As Double
    Dim ParamStr() As String
    Dim ParamDate() As DateTime
    Dim FilterFields() As String
    Dim FilterTexts() As String
End Structure
Public Structure Struct_FP_CONTROLS_COLLECTION
    Dim FieldPrefix As String
    Dim GRID As Struct_FP_GRID_CONTROL_COLLECTION
    Dim BindingNavigator As BindingNavigator
    Dim Btn_Filter As System.Windows.Forms.PictureBox
    Dim Btn_Filter_Line As System.Windows.Forms.PictureBox
    Dim FilterText As System.Windows.Forms.Label
    Dim Btn_Find As System.Windows.Forms.PictureBox
    Dim Btn_ExportToExcel As System.Windows.Forms.PictureBox
    Dim Btn_ImportFromExcel As System.Windows.Forms.PictureBox
    Dim Btn_Print As System.Windows.Forms.PictureBox
    Dim Btn_Add_New As PictureBox
    Dim Btn_Del As PictureBox
    Dim Btn_Up As PictureBox
    Dim Btn_Down As PictureBox
    Dim Btn_DuplicateRecord As PictureBox
End Structure
Public Structure Struct_FP_CONTROL_PROPS
    Dim Visible As Boolean
    Dim Mandatory As Boolean
    Dim Locked As Boolean
    Dim xType_VB As String
    Dim xlength As Integer
    Dim DT_FixText_Key As String
    Dim DT_WHERE2 As String
    Dim DT_ID_Field As String
    Dim F_Format As String
    Dim COLOR_NORMAL_BG As Color
    Dim COLOR_NORMAL_FORE As Color
    Dim COLOR_SELECTED_FORE As Color
    Dim COLOR_LABEL_FORE As Color
    Dim COLOR_LABEL_BG As Color
    Dim BG_Image_Name As String
    Dim Label_Text As String
    Dim ShowInGRID As Boolean
    Dim SavePoint As ENUM_SavePoint_Type
    Dim Forced_NextField As String
    Dim Tag As String
End Structure
Public Structure Struct_FP_FORM_CONTROLS_COLLECTION
    Dim Dlg_Btn_OK As PictureBox
    Dim Dlg_Btn_CANCEL As PictureBox
    Dim Btn_SAVE As PictureBox
    Dim Btn_HELP As PictureBox
    Dim Btn_Default As Control 'Enter lenyomasa eseten ez a control kap egy Click esemenyt.
End Structure
Public Structure Struct_FP_GRID_CONTROL_COLLECTION
    Dim GRID As DataGridView
    Dim Label As Label
    Dim Btn_FooterVisible As PictureBox
    Dim Footer_Panel As Panel
End Structure
Public Structure Struct_FP_SQL_BIND_PARAMS
    Dim RS_ServerObject_Prefix As String
    Dim NameOf_FormDef As String
    Dim NameOf_WhereQuery As String
    Dim NameOf_GRID As String
    Dim NameOf_READ As String
    Dim NameOf_SAVE As String
    Dim NameOf_DEL As String
    Dim TypeOf_WhereQuery As Integer
    Dim TypeOf_GRID As Integer
    Dim TypeOf_READ As Integer
    Dim TypeOf_SAVE As Integer
    Dim TypeOf_DEL As Integer
End Structure
Public Structure Struct_HELPText
    Dim ShortText As String
    Dim Link As String
End Structure
Public Structure Struct_LISTVIEW_Params
    Dim ListViewControl As ListView
    Dim FixText_Key As String
    Dim WHERE2 As String
    Dim SEQ_KEY_HEADERS As String
    Dim CheckBoxes As Boolean
    Dim CountOfColumns As Integer
    Dim ARRAY_HeaderTexts As String()
    Dim ARRAY_ColumnWidths As Integer()
    Dim ARRAY_Aligns As String()
    Dim ARRAY_Formats As String()
    Dim ARRAY_SQL_Column_Names As String()
    Dim ARRAY_SQL_Column_XTypes As String()
    Dim SQL_SELECT As String
    Dim SQL_FROM As String
    Dim SQL_WHERE As String
    Dim SQL_ORDER_BY As String
    Dim ValueMember As String
    Dim REFRESH_Type As ENUM_FP_CONTROL_REFRESH_TYPE
End Structure

Public Structure Struct_PA_CUST
    Dim Base_Groups_ID As Long      'Alap partnerosztaly
    Dim TaxNum_Control_Method As ENUM_Cust_TaxNum_Control_Method
End Structure

Public Structure Struct_PA
    Dim PRODUCT_NAME As String
    Dim INV_PARAMS As Struct_PA_INV_PARAMS
    Dim INV_DIRECT_PARAMS As Struct_PA_INV_DIRECT_PARAMS
    Dim ROUTES_PARAMS As Struct_PA_ROUTES_PARAMS
    Dim ORD_PARAMS As Struct_PA_ORD
    Dim Layout_Params As Struct_PA_Layout_Params
    Dim Report_Params As Struct_PA_Report_Params
    Dim Accounting_Params As Struct_PA_ACC
    Dim Currencies_Params As Struct_PA_CURR
    Dim Owner_Params As Struct_PA_Owner
    Dim System_Params As Struct_PA_SYS
    Dim WEB As Struct_PA_WEB
    'Dim General_Bank_Ok As Boolean              'Param_ALT_BANK_OK
    Dim DOCMAN As Struct_PA_DOCMAN
    Dim CUST As Struct_PA_CUST
    Dim CREDIT_LIMIT_Params As Struct_CREDIT_LIMIT_Params
    Dim NEW_DEVELOPMENT_PARAMS_JSON As String
End Structure
Public Structure Struct_PA_ACC
    Dim ACC_AC_DEB_ID As Integer
    Dim ACC_AC_VENDOR_ID As Integer
End Structure
Public Structure Struct_PA_DOCMAN
    Dim DeleteAfterSave As ENUM_DOCMAN
End Structure
Public Structure Struct_PA_CURR
    Dim Curr_LC_ID As Integer
    Dim Curr_LC_Code As String      '= Wahrungen.KurzName, vagyis a deviza programban hasznalt jele.
    Dim Curr_LC_Sign As String      '= Wahrungen.KurzZeichen, vagyis az, ahogy a devizának a dokumentumokon meg kell jelenni.
    Dim Curr_FC_ID As Integer
    Dim Curr_FC_Code As String
    Dim Curr_FC_Sign As String
End Structure
Public Structure Struct_PA_INV_PARAMS
    Dim INV_Format_ID As Long                                                        'Az alapertelmezett szamla formatum
    Dim Copies As Integer                                                            '2. Szamlazas_Peldanyszam 0-9
    Dim Leporello As Boolean                                                         '3. Szamlazas_Leporello 
    Dim InvFor As ENUM_INV_PARAMS_INV_FOR                                            '4. Szamlazas_SzamlaMindenUgyfelhez
    Dim Rounding As ENUM_INV_PARAMS_ROUNDING                                         '5. Szamlazas_RundungArt    !!!!ENUM
    Dim TopOffset As Integer                                                         '11.Szamlazas_Top_Eltolas
    Dim ManualFiling As Boolean                                                      '17.Szamlazas_KeziIktatas 
    Dim ManualPrinterSelect As Integer                                               '18.Szamlazas_Valaszthato_Nyomtato
    Dim RefRequired As Boolean                                                       '19.Szamlazas_HivBizKotelezo
    Dim Credit_Payment_Method As ENUM_INV_PAYMENT_METHOD                             '23 Credit_Payment_Method
    Dim Debit_Payment_Method As ENUM_INV_PAYMENT_METHOD                              '24 Debit_Payment_Method
    Dim Credit_DueDays As Integer                                                    '26-28 Credit_DueDays
    Dim Debit_DueDays As Integer                                                     '29-31 Debit DueDays
    Dim Subcontracted_Services_Method As ENUM_Subcontracted_Services_Method          '32 Subcontracted Services Method
    Dim Debit_CurrDate_Code As ENUM_DELIVERYDATE_CODE_L                              '33
    Dim Debit_DeliveryDate_Code_L As ENUM_DELIVERYDATE_CODE_L                        '34
    Dim Credit_CurrDate_Code As ENUM_DELIVERYDATE_CODE_L                             '35
    Dim Credit_DeliveryDate_Code_L As ENUM_DELIVERYDATE_CODE_L                       '36
    Dim Periodic_DeliveryDate_Code As ENUM_PERIODIC_DELIVERYDATE                     '37
    Dim Subcontracted_Services_ShowOnInvoice As ENUM_INV_SUBCONTRACTED_SERVICES_SHOW
    Dim DeliveryDate_Code_Head As ENUM_INV_DELIVERYDATE_HEAD_METHOD
    Dim INV_AccDate_From As ENUM_INV_PLANED_PAYMENT_METHOD
    Dim INV_And_Calc_Manage As ENUM_INV_Inv_And_Calc_Manage_Type                     '41
    Dim INV_Line_Handling As ENUM_INV_Line_Handling_Type                             '42
    Dim CreditNote_Sign_Manage As ENUM_INV_CreditNote_Sign_Manage                    '43
    Dim Calc_Sign_Manage As ENUM_INV_Calc_Sign_Manage                                '44
    Dim INV_Save_Origin_To_PDF As ENUM_INV_Save_Origin_To_PDF                    '47
    Dim INV_Save_Origin_To_PDF_Location As String                                   'Param: INV/SAVE_PDF_LOCATION
    Dim INV_Save_Origin_To_PDF_FileName As String                                   'Param: INV/SAVE_PDF_FILENAME
    Dim Deposit_Invoice_Print_Mode As Integer                                       'Param: INV/DEPOS_PRINT_MODE        / 1: Eloleg szla nyomtatasa elott nem kell bank 2: Eloleg szla nyomtatasa csak ha mar a banki utalas megtortent
End Structure
Public Structure Struct_PA_INV_DIRECT_PARAMS
    Dim METHOD As ENUM_INV_DIRECT_METHOD
    Dim APPEND_TO_EXISTING As Boolean
End Structure
Public Structure Struct_PA_Layout_Params
    Dim GRID_FILTER_Button_Pressed As Boolean
End Structure
Public Structure Struct_PA_ORD
    'Dim ORD_STATUS_ID_BASE As Long
    'Dim ORD_L_STATUS_ID_BASE As Long
    Dim PROCESS_HANDLING As String
End Structure
Public Structure Struct_PA_Owner
    Dim CUST_OWNER_ID As Integer
    Dim CUST_OWNER_Name1 As String
    Dim COUNTRY As String
    Dim LANGUAGE As String
End Structure
Public Structure Struct_PA_Report_Params
    Dim ReportPath As String
End Structure
Public Structure Struct_PA_ROUTES_PARAMS
    Dim DailyFee_Rates_ID As Long                   'gl_NapidijID
    Dim DailyFee_Curr_ID As Integer                 'gl_NapidijKtgDevID
    'Dim DailyFeeCurr As String                     'gl_NapidijKtgDev
    Dim RouteStart_Rates_ID As Integer              'gl_InditasID
    Dim CashMove_Rates_ID As Integer                'gl_AtadasID
    Dim Routes_Cards_Cash_ID As Integer             'gl_KeszpenzID
    Dim Routes_Cost_Fuel_ID As Integer              'IND / UZEMANYAG_KTG_ID
    Dim ROUTE_STATUS_ID_BASE As Integer
End Structure
Public Structure Struct_PA_SYS
    Dim JOBS_TIMERINTERVAL As Integer
End Structure
Public Structure Struct_PA_WEB
    Dim WEB_Curr_Manage As ENUM_WEB_Curr_Manage
End Structure
Public Structure Struct_PrintDocs_Options
    Dim NameInList As String
    Dim ReportFileName As String
    Dim ReportType As ENUM_ReportType
    Dim ReportOpenType As ENUM_ReportOpenType
    Dim AskAfterPrint As Boolean
    Dim NumberOfCopies As Integer
    Dim Source As ENUM_ReportSource
End Structure
Public Structure Struct_Report_DataSources
    Dim Refresh_Allways As Boolean
    Dim SQL_Source As String
    Dim Subreport_Name As String
    Dim DT As DataTable
End Structure
Public Structure Struct_Rights
    Dim ORD_Handling As Boolean              'SendungenModifizieren
    Dim ORD_Handling_AfterClosing As Boolean 'SendungenModifizierenNachAbschluss
    Dim ORD_Temp_Handling As Boolean         'SablonModifizieren
    Dim ORD_Handling_All As Boolean          'AlleSendungenMod
    Dim AR_INV_Print As Boolean              'ReDrucken - Kimeno szamla nyotathato
    Dim AR_INV_Handling As Boolean           'ReModifizieren
    Dim Report_Print As Boolean              'SpedibuchDrucken
    Dim AP_INV_Handling As Boolean           'EingangsReModifizieren
    Dim AP_INV_Filing As Boolean             'ReDrucken - Bejovo szamla iktathato
    Dim CUS_Add As Boolean                   'KundenstammErfassen
    Dim CUS_Handling As Boolean              'KundenstammModifizieren
    Dim CUS_Del As Boolean                   'KundenstammLoschen
    Dim CUS_Report As Boolean                'KundenStammDrucken
    Dim CUS_ACC_Handling As Boolean          'KundenStammKreditLimitMod
    Dim GOD_Handling As Boolean              'ArtikelstammBearbeiten
    Dim VCL_Handling As Boolean              'LKWStammBearbeiten
    Dim EMP_Handling As Boolean              'PersonalStammBearbeiten
    Dim EMP_Handling_All As Boolean          'PersonalStammAlleDaten
    Dim PAR_Handling As Boolean              'ParaStammBearbeiten
    Dim Distances_Handling As Boolean        'EntfernungenBearbeiten
    Dim RAT_Handling As Boolean              'TarifenBearbeiten
    Dim CUR_RAT_Handling As Boolean          'WahrungStammBearbeiten
    Dim CUR_Handling As Boolean              'WahrungGrunddatenAndern
    Dim BNK_AC_Handling As Boolean           'BankkontoNrBearbeiten
    Dim PA_Defs_Handling As Boolean          'FixTexteBearbeiten
    Dim USR_Handling As Boolean              'KonfigAendern
    Dim GLNr_Handling As Boolean             'FIBUKontenEditieren
    Dim BNK_Handling As Boolean              'BankBearbeiten
    Dim CBK_Handling As Boolean              'KasseBearbeiten
    Dim INV_Handling_Level As Long           'RechnungDokuStandBis
    Dim CBK_ACC_Handling_Level As Long       'KasseDokuStandBis
    Dim BNK_ACC_Handling_Level As Long       'BankDokuStandBis
    Dim RTS_MAN As Boolean                   'TurenBearbeiten
    Dim RTS_Handling_AfterClosing As Boolean 'TurenBearbeitenNachAbschluss
    Dim WHS_BasData_MAN As Boolean           'LgrGrundDatenBearbeiten
    Dim WHS_Tran_MAN As Boolean              'LgrBewegungenBearbeiten
    Dim ExcelExport As Boolean               'ExcelExport
    Dim WorkInClosedPeriod As Boolean        'WorkInClosedPeriod
    Dim TranBetweenPeriods As Boolean        'TransactionsBetweenPeriods
    Dim CalcModifyAfterBilling As Boolean    'CalcModifyAfterBilling
    Dim DOCMAN_SecurityLevel As Integer      'DOCMAN_SecurityLevel
    Dim UserSpec01 As String                 'UserSpec01
    Dim UserSpec02 As String                 'UserSpec02
    Dim UserSpec03 As String                 'UserSpec03
    Dim UserSpec04 As String                 'UserSpec04
    Dim UserSpec05 As String                 'UserSpec05
    Dim UserSpec06 As String                 'UserSpec06
End Structure

Public Structure Struct_RS
    Dim RS_Obj_Name As String
    Dim RS_ID_FieldName As String
    Dim RS_FROM As String
    Dim RS_WHERE As String
    Dim RS_GROUPBY As String
    Dim RS_ORDERBY As String
    Dim Selected As Boolean
    Dim MaxRecords As Integer
    Dim HWND As Long
End Structure

Public Structure Struct_RS_OUT
    Dim RS_ID As Long
    Dim RECORDCOUNT As Integer
End Structure

Public Structure Struct_SEQ
    Dim SEQ_Key As String
    Dim Number As Long
    Dim Text1 As String
    Dim Text2 As String
    Dim Text3 As String
    Dim Text4 As String
End Structure
Public Structure Struct_Simple_SELECT_OutputParams
    Dim RS_ID As Long
    Dim Selected_ID As Long
    Dim Selected_String As String
    Dim Selected_Long As Long
    Dim Selected_Text As String
    Dim NO_LIMIT_TO_LIST As Boolean
    Dim GotoNextField As Boolean
End Structure
Public Structure Struct_Simple_SELECT_Params
    Dim FPc As FP_Control
    Dim Field_Mandatory As Boolean
    Public Field_TRIM As Boolean
    Public Field_UCASE As Boolean
    Public Field_NOSPACE As Boolean
    Public Field_MULTILINE As Boolean
    Dim FixText_Key As String
    Dim MAXRECORDS As Long
    Dim NoSetText As Boolean
    Dim RS_ID As Long
    Dim Selected_Text As String
    Dim Selected As Boolean
    Dim SQL_WHERE As String
    Dim NoMessageForNoRecord As Boolean
    Dim PressOK As Boolean
    Dim XLength As Integer
End Structure
Public Structure Struct_TRN_Params
    Dim Code As Integer
    Dim LNrCode As String
    Dim LNrSchlussel As String
    Dim Param1 As String
    Dim OldArtikel_Mussfeld As Boolean
    Dim Artikel_Mussfeld As Boolean
    Dim Price_Code As Integer
    Dim Production As Boolean
    Dim Multiclient As Boolean
    Dim Vorzeichen As Integer
    Dim EinAus As Integer
    Dim Kreditor As String
    Dim Debitor As String
    Dim GrundKreditorID As Integer
    Dim GrundDebitorID As Integer
    Dim Remark As String
End Structure
Public Structure Struct_ZDISPO_DEF
    Dim Identifier As String
    Dim StoredProc_1 As String
    Dim Fix_WHERE As String
    Dim DoFilter As String
    Dim DoFilter_WhereQuery As String
    Dim Simple_Select As String
    Dim StoredProc_2 As String
    Dim Report_Name As String
    Dim Report_SQL As String
    Dim Report_OpenType As Integer
    Dim Excel_export As String
    Dim Txt_export As String
    Dim FilePath As String
    Dim StoredProc_3 As String
    Dim Email As String
    Dim Next_Identifier As String
End Structure
Public Structure Struct_ZDISPO_OutputParams
    Dim RS As Struct_Simple_SELECT_OutputParams
End Structure
Public Structure Struct_ZDISPO_Params
    Dim ModuleIdentifier As String
    Dim Parent_FPf As FP_Form
    Dim Identifier As String '=Param1 (a megnyitando lista neve)
    Dim Current_ID As Long 'hivatkozni lehet ra a 'Fix_Where' es a 'Report_WHERE' parameterekben "#_CURRENT_ID_#"-vel
    Dim Current_RS_ID As Long 'hivatkozni lehet ra a 'Fix_Where' es a 'Report_WHERE' parameterekben "#_CURRENT_RS_ID_#"-vel
    Dim RS_ID As Long 'Regen Param3: ha itt 1 volt, akkor nem torolte a Dispo1 tablat. Most: ha RS_ID nem 0, akkor nem nyitja meg az esetlegesen megadott Simple_selectet, hanem ezt az RS_ID hasznalja fel.
    Dim Show_SimpleSelect As Boolean 'Ha RS_ID <> 0, akkor alaphelyzetben nem nyitja meg a SIMPLE_SELECT-et. De ha ez az ertek TRUE, akkor igen. (Tobbszoros valasztas eseten hasznos)
    Dim Selected As Boolean 'Param3.bit 2 (Ha True, akkor a rekordok tobbszoros valasztasnal azonnal ki lesznek jelolve.)
    Dim PressOK As Boolean 'Param3.bit 4 (Ha True, akkor a lista elkeszitese utan automatikusan "lenyomja" a Tovabb gombot.)
    Dim RS_Type As String 'Param4 (Megadja, hogy az Art mezot mivel toltse ki a ZDISPO)
    Dim SQL_WHERE As String 'Param5 (csak az itt megadott feltetelnek megfelelo rekordok jelennek meg.)
    Dim NoMessageForNoRecord As Boolean 'Param6 (Ha ez True, akkor nem jelenik meg a 'Nincs megjelenitendo adat' ablak)
End Structure
Public Structure Struct_CREDIT_LIMIT_Params
    Dim CHK_MODE As ENUM_CREDIT_LIMIT_CHK_MODE
End Structure
#End Region

Public Module FP_Globals

#Region "Versions"
    'Eddigi verziók:
    'Public Const vers = "Selsped Version 1.06 1999.08.17"
    'Public Const vers = "Selsped Version 2.01 2000.09.01"
    'Public Const vers = "Selsped Version 2.02 2000.09.03"
    'Public Const vers = "Selsped Version 2.03 2000.09.17"
    'Public Const vers = "Selsped Version 2.04 2000.10.09"
    'Public Const vers = "Selsped Version 2.05 2000.10.30"
    'Public Const vers = "Selsped Version 2.06 2000.12.04"
    'Public Const vers = "Selsped Version 2.07 2000.12.12"
    'Public Const vers = "Selsped Version 2.08 2001.01.04"
    'Public Const vers = "Selsped Version 2.09 2001.02.15"
    'Public Const vers = "Selsped Version 2.10 2001.02.22"
    'Public Const vers = "Selsped Version 2.11 2001.03.17"
    'Public Const vers = "Selsped Version 2.12 2001.03.24"
    'Public Const vers = "Selsped Version 2.13 2001.04.17"
    'Public Const vers = "Selsped Version 2.14 2001.07.06"
    'Public Const vers = "Selsped Version 2.15 2001.09.03"
    'Public Const vers = "Selsped Version 2.16 2001.10.01"
    'Public Const vers = "Selsped Version 2.17 2002.02.05"
    'Public Const VERS = "Selsped Version 2.18 2002.05.12"
    'Public Const VERS = "Selsped Version 3.01 2005.12.01"
    'Public Const VERS = "Selsped Version 4.01 2007.04.12"
    'Public Const VERS = "Selsped Version 4.02 2007.06.25"
    'Public Const VERS = "4.02 SP: 1.01 2007.07.13."
    'Public Const VERS = "4.021 SP: 0.00 2007.07.31."
    'Public Const VERS = "4.022 SP: 1.01 2007.08.27."
    'Public Const VERS = "4.023 SP: 0.00 2007.09.03."
    'Public Const VERS = "4.023 SP: 1.01 2007.09.24."
    'Public Const VERS = "4.024"
    'Public Const VERS = "4.025"
    'Public Const VERS = "4.026"
    'Public Const VERS = "4.027"
    'Public Const VERS = "4.028"
    'Public Const VERS = "4.029"
    'Public Const VERS = "4.030"
    'Public Const VERS = "4.031"
    'Public Const VERS = "4.032"
    'Public Const VERS = "4.033"
    'Public Const VERS = "4.034"
    'Public Const VERS = "4.035"
    'Public Const VERS = "4.036"
    'Public Const VERS = "4.038"
    'Public Const VERS = "4.039"
    'Public Const VERS = "4.040"
    'Public Const VERS = "4.041"
    'Public Const VERS = "4.043"
    'Public Const VERS = "4.044"
    'Public Const VERS = "4.045"
    'Public Const VERS = "4.046"
    'Public Const VERS = "4.047"
    'Public Const VERS = "4.048"
    'Public Const VERS = "4.049"
    'Public Const VERS = "SEL_2017_01"
    'Public Const VERS = "SEL_2017_02"       '2017-05-23
    'Public Const VERS = "SEL_2017_03"       '2017-09-13
    'Public Const VERS = "SEL_2017_04"       '2017-11-05
    'Public Const VERS = "SEL_2018_01"       '2017-11-05
    'Public Const VERS = "SEL_2018_02"       '2018-11-12
    'Public Const VERS = "SEL_2019_01"       '2019-04-12
    Public Const VERS = "SEL_2019_02"
    Public VERS_LOCAL_DB As Integer = -1
    Public Const SRV = "0.00"
    Public Const ProgramName$ = "SELEXPED"
    Public Const ProductNumber$ = "10001"
#End Region
#Region "gl_.. variables"
    Public gl_Data_Binded As Boolean = True
    Public gl_ZDISPO_KivalasztottID As Long
    Public gl_ZDISPO_KivalasztottString As String
    Public gl_ZDISPO_KivalasztottLong As Long
    Public gl_Doit As Boolean
#End Region

    Public gl_SIMPLE_SELECT_OutputParams As New Struct_Simple_SELECT_OutputParams

#Region "General global variables"
    Public gl_FPApp As FP_App
    Public Terminal As String
    Public Terminals_ID As Long
    Public SelUser As Long
    Public UserKurzName As String
    Public UserName As String
    Public UserName_Foreign_Lang_1 As String
    Public UserName_Foreign_Lang_2 As String
    Public UserEmail As String
    Public UserPhone1 As String
    Public UserPhone2 As String
    Public CL_Chk_DataTable As DataTable
    Public Organisation_Rights_IDS As String
#End Region
#Region "Variables for Selester"
    Public SelesterName As String = "Selester Kft"
    Public SelesterCountry As String = "H"
    Public SelesterZIP As String = "1113"
    Public SelesterCity As String = "Budapest"
    Public SelesterAddress As String = "Kökörcsin u. 11"
    Public SelesterTel As String = "+36-1-372-00-61, 62"
    Public SelesterFax As String = "+36-1-372-00-63"
    Public SelesterHotline As String = "+36-20-9213-679"
    Public SelesterHomepage As String = "www.selester.hu"
    Public SelesterEMail As String = "info@selester.hu"
    Public SelesterTaxnumber As String = "12926626"
#End Region

    Public Sub DoErrorMsgBox_Without_SQL_Connection(ByVal ErrorLocation As String, ByVal ErrNumber As Long, ByVal ErrDescriptions As String)
        CURSOR_SHOW_DEFAULT()
        MsgBox(ErrorLocation + " Internal Error." + vbCrLf + vbCrLf + "Error Number: " + ErrNumber.ToString + vbCrLf + ErrDescriptions)
    End Sub

    Public Function MOUSE_GET_LEFT_BUTTON_PRESSED() As Boolean
        Return (Control.MouseButtons = MouseButtons.Left)
    End Function

    Public Function MOUSE_GET_RIGHT_BUTTON_PRESSED() As Boolean
        Return (Control.MouseButtons = MouseButtons.Right)
    End Function

    Function EXCEL_INT_TO_DATE(ByVal ExcelInt As Integer) As DateTime
        Dim OUT As DateTime = NULLDATE

        If ExcelInt <> 0 Then
            OUT = DateAdd(DateInterval.Day, ExcelInt, DateSerial(1899, 12, 30))
        End If

        Return OUT
    End Function

    Function EXCEL_GET_DATE_INT(ByVal MyDate As Date)
        Return DateDiff(DateInterval.Day, DateSerial(1899, 12, 30), MyDate)
    End Function

    Public Function nz(ByVal MyVal As Object, ByVal MyRetVal As Object) As Object
        Dim OUT As Object = ""

        If IsDBNull(MyVal) Then
            OUT = MyRetVal
        ElseIf (MyVal Is Nothing) Then
            OUT = MyRetVal
        Else
            OUT = MyVal
        End If

        nz = OUT
    End Function

    Public Sub Form_Move_To_CenterScreen(ByVal MyForm As Form)
        If Not (MyForm Is Nothing) Then
            Dim CurrentScreen As Screen = Screen.FromPoint(MyForm.PointToScreen(New Point(0, 0)))

            Dim x As Integer = (CurrentScreen.Bounds.Width - MyForm.Width) / 2
            Dim y As Integer = (CurrentScreen.Bounds.Height - MyForm.Height) / 2

            If x < 0 Then
                x = 0
            End If

            If y < 0 Then
                y = 0
            End If

            MyForm.Location = New Point(x, y)
        End If
    End Sub

    Public Function Form_Border_Width(ByVal MyForm As Form) As Integer
        Dim OUT As Integer = 0

        If Not (MyForm Is Nothing) Then
            OUT = MyForm.PointToScreen(New Point(0, 0)).X - MyForm.Left
        End If

        Return OUT
    End Function

    Public Function Form_Handle(MyFrm As Form) As Long
        Dim OUT As Long = 0

        If Not (MyFrm Is Nothing) Then
            Try
                OUT = MyFrm.Handle

            Catch ex As Exception
                'Nothing to do
            End Try
        End If

        Return OUT
    End Function

    Public Function Form_Title_Height(ByVal MyForm As Form) As Integer
        'Csak a form cim resze
        Dim OUT As Integer = 0

        If Not (MyForm Is Nothing) Then
            OUT = MyForm.PointToScreen(New Point(0, 0)).Y - MyForm.Top
        End If

        Return OUT
    End Function

    Public Function Form_Header_Height(ByVal MyForm As Form) As Integer
        'Form cim resze a menuvel egyutt
        Dim OUT As Integer = 0

        If Not (MyForm Is Nothing) Then
            OUT = MyForm.PointToScreen(New Point(0, 0)).Y - MyForm.Top
            If Not (MyForm.MainMenuStrip Is Nothing) Then
                OUT += MyForm.MainMenuStrip.Height
            End If
        End If

        Return OUT
    End Function

    Public Function FPc_HAS_FIELD(ByVal FPc As FP_Control) As Boolean
        Dim OUT As Boolean = False

        If Not (FPc Is Nothing) Then
            If Not (FPc.c Is Nothing) Then
                OUT = True
            End If
        End If

        FPc_HAS_FIELD = OUT
    End Function

    Public Function FPc_HAS_LABEL(ByVal FPc As FP_Control) As Boolean
        Dim OUT As Boolean = False

        If Not (FPc Is Nothing) Then
            If Not (FPc.c_Label Is Nothing) Then
                OUT = True
            End If
        End If

        FPc_HAS_LABEL = OUT
    End Function

    Public DBNullDate As DateTime = DateSerial(1900, 1, 1)
    Public Const NULLDATE As Date = #12:00:00 AM#

#Region "DICTIONARIES"
    Function DIC_GET(ByVal Key As String, ByVal DIC As Dictionary(Of String, String)) As String
        Dim OUT As String = ""

        If DIC Is Nothing Then
            DoErrorMsgBox_Without_SQL_Connection("FP_Globals.DIC_GET", 0, "DIC is nothing.")
        Else
            If DIC.ContainsKey(Key) Then
                OUT = DIC(Key)
            End If
        End If

        DIC_GET = OUT
    End Function
#End Region

    Public Function Vorhanden(ByVal MyDir As String) As Boolean
        Dim OUT As Boolean = False
        Dim wchar As String = String.Empty
        Try
            If Trim(MyDir) <> String.Empty Then
                If InStr(MyDir, "*") > 0 Or InStr(MyDir, "?") > 0 Then
                    Return False
                Else
                    OUT = System.IO.File.Exists(MyDir)
                End If
            End If
        Catch ex As Exception
            DoErrorMsgBox_Without_SQL_Connection("FP_Globals.Vorhanden", Err.Number, Err.Description)
        End Try
        Return OUT
    End Function

    Public Function COLOR_GET_STR_FROM_COLOR(ByVal MyColor As Color) As String
        COLOR_GET_STR_FROM_COLOR = MyColor.R.ToString + "; " + MyColor.G.ToString + "; " + MyColor.B.ToString
    End Function

    Public Sub Clipboard_SET_TEXT(MyText As String)
        If nz(MyText, "") = "" Then
            My.Computer.Clipboard.Clear()
        Else
            My.Computer.Clipboard.SetText(MyText)
        End If
    End Sub

    Public Function Clipboard_GET_TEXT() As String
        Return nz(My.Computer.Clipboard.GetText, "")
    End Function

    Public Function COLOR_GET_FROM_STR(ByVal MyStr As String, ByVal COLOR_Default As Color, ByVal FieldName As String, ByRef OUT_Color As Color, Optional ByVal WithDialog As Boolean = True) As Boolean
        Dim OUT As Boolean = True
        Dim Color_STR() As String = Split(Replace(Replace(MyStr, "(", ""), ")", ""), ";")

        OUT_Color = COLOR_Default

        If UBound(Color_STR) = 0 Then
            If MyStr > "" Then
                OUT = False
            End If
        Else
            If UBound(Color_STR) <> 2 Then
                OUT = False
            Else
                Dim R As Integer = Val(Color_STR(0))
                Dim G As Integer = Val(Color_STR(1))
                Dim B As Integer = Val(Color_STR(2))

                If R < 0 Or R > 255 Or G < 0 Or G > 255 Or B < 0 Or B > 255 Then
                    OUT = False
                Else
                    If Color.FromArgb(R, G, B) = COLOR_Default Then
                        'A most kovetkezo resz azert kell, mert a COLORING eljaras ugy donti el, hogy egyedi szinbeallitasrol van-e szo,
                        'hogy megnezi, hogy az FP_Control szine mas-e, mint a default szin.
                        'Ha nem mas, akkor a szabvanyos szineket hasznalja annak ellenere, hogy mi megadtuk a szint.
                        'A default szin fix megadasanak eppen az az ertelme, hogy automatikusan ne valtozzon a szin.
                        'Ezek a gondolatok leginkabb a hatterszin beallitasanal ertelmesek.
                        If B = 255 Then
                            B = 254
                        Else
                            B += 1
                        End If
                    End If
                    OUT_Color = Color.FromArgb(R, G, (B + 1) \ 255)
                    OUT_Color = Color.FromArgb(R, G, B)
                    If OUT_Color = COLOR_Default Then
                        OUT_Color = Color.FromArgb(Val(Color_STR(0)), Val(Color_STR(1)), Val(Color_STR(2)))
                    End If
                End If
            End If
        End If

        If Not OUT Then
            If WithDialog Then
                MsgBox(String.Format("FP_Globals.COLOR_GET_FROM_STR: MyStr for Field {0} is invalid. (Use Format: '(<RED>; <GREEN>; <BLUE>)", FieldName))
                OUT_Color = Color.FromArgb(255, 255, 255)
            End If
        End If

        COLOR_GET_FROM_STR = OUT
    End Function

    Public Function CONTROLS_Is_c_On_TabControl(ByVal c As Control, ByRef OUT_ParentTabControl As TabControl, ByRef OUT_ParentTabPage As TabPage) As Boolean
        Dim OUT As Boolean = False
        Dim Parent_c As Control = Nothing

        Parent_c = c.Parent
        Do While Not (Parent_c Is Nothing Or (TypeOf (Parent_c) Is Form))
            If TypeOf (Parent_c) Is TabPage Then
                OUT_ParentTabPage = Parent_c
                OUT_ParentTabControl = Parent_c.Parent

                OUT = True

                Exit Do
            Else
                Parent_c = Parent_c.Parent
            End If
        Loop

        CONTROLS_Is_c_On_TabControl = OUT
    End Function

    Public Sub CONTROLS_SPLITCONTAINER_SET_NAME_OF_PANELS(ByVal MySplitContainer As SplitContainer)
        With MySplitContainer
            If .Panel1.Name = "" Then
                .Panel1.Name = .Name + "_Panel1"
            End If
            If .Panel2.Name = "" Then
                .Panel2.Name = .Name + "_Panel2"
            End If
        End With
    End Sub

    Public Sub CONTROLS_WRITE_ALL_CONTROLS_TO_DIC(ByVal O As Object, ByRef DIC As Dictionary(Of String, Control))
        If DIC Is Nothing Then
            DIC = New Dictionary(Of String, Control)
        End If

        DIC.Clear()

        If Not (O Is Nothing) Then

            Dim c As Control
            Dim Akt_c As Control
            Dim ST(0) As Control
            Dim ST_Current(0) As Integer
            Dim STp As Integer = 0

            For Each c In O.Controls
                If c.Name > "" Then
                    DIC.Add(c.Name, c)
                    If TypeOf (c) Is SplitContainer Then
                        CONTROLS_SPLITCONTAINER_SET_NAME_OF_PANELS(c)
                    End If
                End If
                If c.Controls.Count > 0 Then
                    If Not TypeOf (c) Is Microsoft.Reporting.WinForms.ReportViewer Then
                        ST(0) = c
                        STp = 0
                        ST_Current(0) = 0
                        Do
                            If ST_Current(STp) < ST(STp).Controls.Count Then
                                Akt_c = ST(STp).Controls(ST_Current(STp))
                                If Akt_c.Name > "" Then
                                    DIC.Add(Akt_c.Name, Akt_c)
                                    If TypeOf (Akt_c) Is SplitContainer Then
                                        CONTROLS_SPLITCONTAINER_SET_NAME_OF_PANELS(Akt_c)
                                    End If
                                End If

                                If Akt_c.GetType.ToString.ToUpper <> "MICROSOFT.REPORTING.WINFORMS.REPORTVIEWER" Then
                                    If ST(STp).Controls(ST_Current(STp)).Controls.Count > 0 Then
                                        STp += 1
                                        ReDim Preserve ST(STp)
                                        ReDim Preserve ST_Current(STp)
                                        ST(STp) = ST(STp - 1).Controls(ST_Current(STp - 1))
                                        ST_Current(STp) = -1
                                    End If
                                End If
                                ST_Current(STp) += 1
                            End If

                            If ST_Current(STp) >= ST(STp).Controls.Count Then
                                STp -= 1
                                If STp >= 0 Then
                                    ST_Current(STp) += 1
                                End If
                            End If
                        Loop While STp >= 0
                    End If
                End If
            Next
        End If
    End Sub

    Public Sub CONTROLS_REMOVE_CHILDCONTROLS_FROM_DIC(ByVal O As Object, ByRef DIC As Dictionary(Of String, Control))
        If DIC Is Nothing Then
            DoErrorMsgBox_Without_SQL_Connection("FP_Globals.CONTROLS_REMOVE_CHILDCONTROLS_FROM_DIC", 0, "DIC is nothing.")
        Else
            Dim DIC_Children As New Dictionary(Of String, Control)

            CONTROLS_WRITE_ALL_CONTROLS_TO_DIC(O, DIC_Children)

            Dim AktKey As String

            For Each AktKey In DIC_Children.Keys
                DIC.Remove(AktKey)
            Next
        End If
    End Sub


#Region "DATAFORMATS"
    Public Format_Date_Splitter As String = "/"
    Public Format_Date_LengthOfYear As Long = 4
    Public Format_Date_Order As String = "DMY"
    Public Format_Time_Splitter As String = ":"
    Public Format_Float_DecimalPoint As String = ","

    'Public Font_NORMAL As New System.Drawing.Font("Tahoma", 9)
    Public Font_NORMAL As New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
    Public Font_FIXED_SMALL As New System.Drawing.Font("Courier New", 8.0!, FontStyle.Regular, GraphicsUnit.Point, CType(238, Byte))

    'Public COLORS_FIELD_LOCKED_BG As Color = Color.LemonChiffon
    Public COLORS_NULL As Color = Color.FromArgb(255, 255, 254)

    Public COLORS_FIELD_NORMAL_BG As Color = Color.FromArgb(255, 255, 255)
    Public COLORS_FIELD_NORMAL_FORE As Color = Color.FromArgb(0, 0, 0)
    Public COLORS_FIELD_CURRENT_BG As Color = Color.FromArgb(200, 255, 200)
    Public COLORS_FIELD_CURRENT_FORE As Color = Color.FromArgb(255, 255, 255)
    Public COLORS_FIELD_LOCKED_BG As Color = Color.FromArgb(245, 245, 245) 'Color.FromArgb(223, 223, 223) 'Color.FromArgb(254, 249, 207)
    Public COLORS_FIELD_INVALID_BG As Color = Color.FromArgb(255, 200, 200)
    Public COLORS_FIELD_SELECTED_FORE As Color = Color.FromArgb(0, 0, 0)
    Public COLORS_LABEL_BG As Color = Color.FromArgb(105, 105, 105)
    Public COLORS_LABEL_FORE As Color = Color.FromArgb(255, 255, 255)
    Public COLORS_GRID_SELECTEDROW_BG As Color = Color.FromArgb(254, 249, 207)
    Public COLORS_GRID_FILTER_BG As Color = Color.FromArgb(215, 228, 242)
    Public COLORS_GRID_FILTER_FORE As Color = Color.Black

    Public Function Text_Remove_DoubleSpaces(MyText As String) As String
        Dim OUT As String = Replace(MyText, "  ", " ")

        While InStr(OUT, "  ") > 0
            OUT = Replace(MyText, "  ", " ")
        End While

        Return OUT
    End Function

    Public Function TREEVIEW_get_Node_Key_from_Elements(ByVal Level As Integer, ByVal ID As Long) As String
        Return String.Format("{0}_{1}", Level, ID)
    End Function

    Public Function TREEVIEW_NODE_GET_UNDERLINE(MyNode As TreeNode) As Boolean
        Dim OUT As Boolean = False

        If Not (MyNode Is Nothing) Then
            If Not (MyNode.NodeFont Is Nothing) Then
                OUT = MyNode.NodeFont.Underline
            End If
        End If

        Return OUT
    End Function

    Public Sub TREEVIEW_NODE_SET_UNDERLINE(TV As TreeView, MyNode As TreeNode, Let_Underline As Boolean)
        Dim Curr_UnderLine As Boolean = False

        If Not (MyNode Is Nothing) Then
            With MyNode
                If Not (.NodeFont Is Nothing) Then
                    Curr_UnderLine = .NodeFont.Underline
                End If
                If Curr_UnderLine <> Let_Underline Then
                    Dim MyFont As Font = New Font(TV.Font, IIf(Let_Underline = False, FontStyle.Regular, FontStyle.Underline))
                    .NodeFont = MyFont
                End If
            End With
        End If
    End Sub

    Public Function TREEVIEW_Node_GET_Direct_From_TV(TV As TreeView, Name_of_Node As String) As TreeNode
        Dim OUT As TreeNode = Nothing

        If Not (TV Is Nothing) Then
            If TV.Nodes.Count > 0 Then
                Dim Parent_Node As TreeNode = Nothing
                Dim Current_Node As TreeNode = TV.Nodes(0)
                Dim Current_RootNode As TreeNode = Current_Node

                Do
                    If (Current_Node.Parent Is Nothing) Then
                        Current_RootNode = Current_Node
                    End If

                    If Current_Node.Name = Name_of_Node Then
                        OUT = Current_Node
                        Exit Do
                    End If

                    If Current_Node.Nodes.Count > 0 Then
                        Parent_Node = Current_Node
                        Current_Node = Current_Node.Nodes(0)
                    Else
                        Dim Next_Node As TreeNode = Current_Node.NextNode

                        While ((Next_Node Is Nothing) And Not (Current_Node Is Nothing))
                            Current_Node = Parent_Node
                            If Not (Current_Node Is Nothing) Then
                                Parent_Node = Current_Node.Parent
                                Next_Node = Current_Node.NextNode
                            End If
                        End While

                        Current_Node = Next_Node
                    End If

                Loop While Not (Current_Node Is Nothing)
            End If
        End If

        Return OUT
    End Function

    Public Function getDateFromStr(ByVal MyStr As String, ByRef OUT_DateTime As DateTime) As Boolean
        Dim OUT As Boolean = False
        Dim PosOfYear As Long
        Dim PosOfMonth As Long
        Dim PosOfDay As Long
        Dim Splitted() As String
        Dim Splitted_Date() As String
        Dim Splitted_Time() As String

        OUT_DateTime = Nothing
        MyStr = Text_Remove_DoubleSpaces(MyStr)
        If Trim(MyStr) = "" Then
            OUT = True
        Else
            Splitted = Split(MyStr, " ")
            If UBound(Splitted) < 1 Then
                ReDim Preserve Splitted(1)
            End If

            PosOfYear = InStr(Format_Date_Order, "Y") - 1
            PosOfMonth = InStr(Format_Date_Order, "M") - 1
            PosOfDay = InStr(Format_Date_Order, "D") - 1
            If PosOfYear > -1 And PosOfMonth > -1 And PosOfDay > -1 Then
                Splitted_Date = Split(Splitted(0), Format_Date_Splitter)
                If UBound(Splitted_Date) < 1 Then
                    Splitted_Date = Split(Splitted(0), "-")
                End If
                If UBound(Splitted_Date) < 1 Then
                    Splitted_Date = Split(Splitted(0), "\")
                End If
                If UBound(Splitted_Date) < 1 Then
                    Splitted_Date = Split(Splitted(0), "/")
                End If
                If UBound(Splitted_Date) < 1 Then
                    Splitted_Date = Split(Splitted(0), ".")
                End If
                If UBound(Splitted_Date) > 0 Then
                    If UBound(Splitted_Date) < 2 Then
                        ReDim Preserve Splitted_Date(2)
                        Select Case PosOfYear
                            Case 0
                                Splitted_Date(2) = Splitted_Date(1)
                                Splitted_Date(1) = Splitted_Date(0)
                                Splitted_Date(0) = (Now.Year).ToString

                            Case 1
                                Splitted_Date(2) = Splitted_Date(1)
                                Splitted_Date(1) = (Now.Year).ToString

                            Case 2
                                Splitted_Date(2) = (Now.Year).ToString
                        End Select
                    End If

                    If Splitted_Date(PosOfYear).Length < 4 Then
                        Dim wY As Integer = Val(Microsoft.VisualBasic.Strings.Right(Splitted_Date(PosOfYear), 2))
                        If wY >= 0 And wY <= 50 Then
                            Splitted_Date(PosOfYear) = Mid((Now.Year).ToString, 1, 4 - Splitted_Date(PosOfYear).Length) + Splitted_Date(PosOfYear)
                        Else
                            Splitted_Date(PosOfYear) = Mid((Now.Year - 100).ToString, 1, 4 - Splitted_Date(PosOfYear).Length) + Splitted_Date(PosOfYear)
                        End If
                    End If

                    If IsNumeric(Splitted_Date(0)) And IsNumeric(Splitted_Date(1)) And IsNumeric(Splitted_Date(2)) Then
                        Splitted_Date(0) = Replace(Replace(Splitted_Date(0), ".", ""), ",", "")
                        Splitted_Date(1) = Replace(Replace(Splitted_Date(1), ".", ""), ",", "")
                        Splitted_Date(2) = Replace(Replace(Splitted_Date(2), ".", ""), ",", "")

                        Splitted_Time = Split(Splitted(1), Format_Time_Splitter)
                        If UBound(Splitted_Time) < 2 Then
                            ReDim Preserve Splitted_Time(2)
                        End If

                        Dim YearValue As Integer = Val(Splitted_Date(PosOfYear))
                        Dim MonthValue As Integer = Val(Splitted_Date(PosOfMonth))
                        Dim DayValue As Integer = Val(Splitted_Date(PosOfDay))
                        Dim HourValue As Integer = Val(Splitted_Time(0))
                        Dim MinValue As Integer = Val(Splitted_Time(1))

                        If MonthValue >= 1 And MonthValue <= 12 And DayValue >= 1 And DayValue <= 31 And HourValue >= 0 And HourValue <= 23 And MinValue >= 0 And MinValue <= 59 Then
                            Try
                                OUT_DateTime = DateSerial(YearValue, MonthValue, DayValue)
                                If HourValue <> 0 Then
                                    OUT_DateTime = DateAdd(DateInterval.Hour, HourValue, OUT_DateTime)
                                End If
                                If MinValue <> 0 Then
                                    OUT_DateTime = DateAdd(DateInterval.Minute, MinValue, OUT_DateTime)
                                End If

                                OUT = True

                            Catch ex As Exception
                                OUT = False
                            End Try
                        End If
                    End If
                End If
            End If
        End If

        getDateFromStr = OUT
    End Function
    Public Function getStrDate(ByVal MyDate As DateTime) As String
        Dim OUT As String = ""
        Dim YearStr As String

        If MyDate = Nothing Or MyDate = DBNullDate Then
            getStrDate = ""
        Else
            YearStr = Right((MyDate.Year).ToString, Format_Date_LengthOfYear)
            OUT = Mid(Format_Date_Order, 1, 1) + Format_Date_Splitter + Mid(Format_Date_Order, 2, 1) + Format_Date_Splitter + Mid(Format_Date_Order, 3, 1)
            OUT = Replace(OUT, "Y", YearStr)
            OUT = Replace(OUT, "M", Right("0" + MyDate.Month.ToString, 2))
            OUT = Replace(OUT, "D", Right("0" + (MyDate.Day).ToString, 2))
            If MyDate.Hour > 0 Or MyDate.Minute > 0 Then
                OUT += " " + MyDate.Hour.ToString + Format_Time_Splitter + Right("0" + MyDate.Minute.ToString, 2)
            End If
        End If

        getStrDate = OUT
    End Function
    Public Function getFloatFromStr(ByVal MyStr As String, ByRef OUT_Float As Double) As Boolean
        Dim OUT As Boolean = False

        OUT_Float = 0
        If Trim(MyStr) = "" Then
            OUT = True
        Else
            MyStr = Replace(MyStr, ",", ".")

            Do
                MyStr = Replace(MyStr, Chr(160), "")
            Loop While InStr(MyStr, Chr(160)) > 0

            Do
                MyStr = Replace(MyStr, " ", "")
            Loop While InStr(MyStr, " ") > 0

            OUT_Float = Val(MyStr)
            OUT = True
        End If

        getFloatFromStr = True
    End Function
    Public Function getIntFromStr(ByVal MyStr As String, ByRef OUT_Int As Long) As Boolean
        Dim OUT As Boolean = False

        OUT_Int = 0
        If Trim(MyStr) = "" Then
            OUT = True
        Else
            OUT_Int = ValLong(MyStr)
            OUT = True
        End If

        getIntFromStr = True
    End Function
    Public Function getStrFloat(ByVal MyFloat As Double) As String
        Dim OUT As String = ""

        OUT = MyFloat.ToString
        OUT = Replace(OUT, ".", Format_Float_DecimalPoint)

        getStrFloat = OUT
    End Function
    Public Function getStrInt(ByVal MyInt As Long) As String
        Dim OUT As String = ""

        OUT = MyInt.ToString

        getStrInt = OUT
    End Function
    Public Function DBFORMAT_get_DbStr_From_Date(ByVal MyDate As DateTime) As String
        Dim OUT As String = ""

        If MyDate.Year >= 1901 And MyDate.Year <= 2099 Then
            OUT = MyDate.Year.ToString + MyDate.Month.ToString.PadLeft(2, "0") + MyDate.Day.ToString.PadLeft(2, "0") + "T" + MyDate.Hour.ToString.PadLeft(2, "0") + MyDate.Minute.ToString.PadLeft(2, "0") + MyDate.Second.ToString.PadLeft(2, "0")
        End If

        DBFORMAT_get_DbStr_From_Date = OUT
    End Function

    Public Function DBFORMAT_get_EmptyValue(ByVal xtype_VB As String) As String
        DBFORMAT_get_EmptyValue = DBFORMAT_from_OBJECT("", "", xtype_VB)
    End Function
    Public Function DBFORMAT_TO_Object(ByVal DB_val As String, ByVal xType_VB As String) As Object
        Dim OUT As Object = CStr("")

        Select Case xType_VB
            Case ""
                OUT = DB_val
            Case "INT"
                Try 'konkretan az overflow veszelye miatt lett berakva a try
                    OUT = CInt(Val(DB_val))

                Catch ex As Exception
                    OUT = 0
                End Try

            Case "FLOAT"
                Try 'konkretan az overflow veszelye miatt lett berakva a try
                    OUT = CDbl(Val(DB_val))

                Catch ex As Exception
                    OUT = 0
                End Try

            Case "DATETIME"
                If DB_val = "" Then
                    OUT = CDate(NULLDATE)
                Else
                    OUT = CDate(DBFORMAT_get_Date_From_DbStr(DB_val, ""))
                End If
            Case "BIT"
                Select Case Trim(DB_val)
                    Case "" : OUT = False
                    Case "0" : OUT = False
                    Case "1" : OUT = True

                    Case Else
                        OUT = False
                        DoErrorMsgBox_Without_SQL_Connection("FP_Globals.DBFORMAT_TO_Object", 0, String.Format("Invalid value '{0}' for {1} in DBFORMAT.", DB_val, xType_VB))
                End Select
            Case Else
                DoErrorMsgBox_Without_SQL_Connection("FP_Globals.DBFORMAT_TO_Object", 0, String.Format("Unknown xType_VB '{0}'", xType_VB))
        End Select

        DBFORMAT_TO_Object = OUT
    End Function

    Public Function DBFORMAT_get_Date_From_DbStr(ByVal DateDbStr As String, Optional ByRef OUT_NullIfNull As String = "") As DateTime
        Dim OUT As DateTime = Nothing

        DateDbStr = DateDbStr.PadRight(20, " ")
        OUT_NullIfNull = ""

        Dim MyYear As Integer = Val(DateDbStr.Substring(0, 4))
        Dim MyMonth As Integer = Val(DateDbStr.Substring(4, 2))
        Dim MyDay As Integer = Val(DateDbStr.Substring(6, 2))
        Dim MyHour As Integer = Val(DateDbStr.Substring(9, 2))
        Dim MyMinute As Integer = Val(DateDbStr.Substring(11, 2))
        Dim MySecond As Integer = Val(DateDbStr.Substring(13, 2))

        If MyYear >= 1901 And MyYear <= 2099 _
           And MyMonth >= 1 And MyMonth <= 12 _
           And MyDay >= 1 And MyDay <= 31 _
           And MyHour >= 0 And MyHour <= 23 _
           And MyMinute >= 0 And MyMinute <= 59 _
           And MySecond >= 0 And MySecond <= 59 Then
            If DateTime.DaysInMonth(MyYear, MyMonth) >= MyDay Then
                OUT = New DateTime(MyYear, MyMonth, MyDay, MyHour, MyMinute, MySecond)
            End If
        Else
            OUT_NullIfNull = "Null"
        End If

        DBFORMAT_get_Date_From_DbStr = OUT
    End Function
#End Region
#Region "FIELDPROPS_BASE"
    Public Function FIELD_UNKNOWN_PROPS() As Struct_FP_CONTROL_PROPS
        Dim OUT As Struct_FP_CONTROL_PROPS = Nothing

        With OUT
            .Visible = True
            .Mandatory = False
            .Locked = False
            .xType_VB = ""
            .xlength = 0
            .DT_FixText_Key = ""
            .DT_WHERE2 = ""
            .F_Format = ""
            .COLOR_NORMAL_BG = Color.White
            .BG_Image_Name = ""
            .Label_Text = ""
            .ShowInGRID = False
            .SavePoint = ENUM_SavePoint_Type.NONE
            .Forced_NextField = ""
        End With

        FIELD_UNKNOWN_PROPS = OUT
    End Function

    Public FIELD_UNKNOWN_Default_Field_Rectangle As New Rectangle(101, 0, 120, 22)
    Public FIELD_UNKNOWN_Default_Label_Rectangle As New Rectangle(0, 0, 100, 22)
#End Region


    Public Function DATE_WITHOUT_TIME(ByVal MyDate As DateTime) As DateTime
        Dim OUT As DateTime = NULLDATE

        If MyDate <> NULLDATE Then
            OUT = DateSerial(MyDate.Year, MyDate.Month, MyDate.Day)
        End If

        Return OUT
    End Function

    Public Function DATE_LAST_DAY_OF_MONTH_LASTDATE(MyDate As DateTime) As DateTime
        Dim OUT As DateTime = MyDate

        If OUT <> NULLDATE Then
            OUT = DateSerial(MyDate.Year, MyDate.Month, 1)
            OUT = DateAdd(DateInterval.Month, 1, OUT)
            OUT = DateAdd(DateInterval.Day, -1, OUT)
        End If

        Return OUT
    End Function

    Public Function DBFORMAT_from_SqlCommParameter(ByVal SQLParam As System.Data.SqlClient.SqlParameter, ByVal xtype_VB As String) As String
        Dim OUT As String = ""

        If IsDBNull(SQLParam.Value) Then
            OUT = ""
        Else
            Select Case xtype_VB
                Case "" : OUT = (SQLParam.Value).ToString
                Case "INT" : OUT = (SQLParam.Value).ToString
                Case "FLOAT" : OUT = Replace((SQLParam.Value).ToString, ",", ".")
                Case "BIT" : OUT = IIf(SQLParam.Value = True, "1", "0")
                Case "DATETIME"
                    OUT = DBFORMAT_get_DbStr_From_Date(SQLParam.Value)

                Case Else
                    MsgBox(String.Format("FP.DBFORMAT_from_SqlCommParameter: Unknown datatype (xtype_VB = {0})", xtype_VB))
                    OUT = ""
            End Select
        End If

        DBFORMAT_from_SqlCommParameter = OUT
    End Function
    Public Function DBFORMAT_from_OBJECT(ByVal o As Object, ByVal FieldName As String, ByVal xtype_VB As String) As String
        Dim OUT As String = ""

        Select Case xtype_VB
            Case "" : OUT = nz(o, "").ToString
            Case "DATETIME"
                If IsNull(o) Then
                    OUT = DBFORMAT_get_DbStr_From_Date(CType(Nothing, DateTime))
                Else
                    If IsDate(o) Then
                        OUT = DBFORMAT_get_DbStr_From_Date(CType(o, DateTime))
                    Else
                        OUT = DBFORMAT_get_DbStr_From_Date(CType(Nothing, DateTime))
                    End If
                End If
            Case "INT" : OUT = (CType(Val(nz(o, 0)), Long)).ToString

            Case "FLOAT"
                OUT = Replace(CType(Val(nz(o, 0.0)), Double).ToString, ",", ".")

            Case "BIT"
                If o Is Nothing Then
                    OUT = "0"
                ElseIf o.ToString = "" Then
                    OUT = "0"
                Else
                    OUT = IIf(CType(nz(o, False), Boolean) = True, "1", "0")
                End If
            Case Else
                MsgBox(String.Format("FP.DBFORMAT_from_OBJECT: field {0} has an unknown datatype ({1})", FieldName, xtype_VB))
                OUT = ""
        End Select

        DBFORMAT_from_OBJECT = OUT
    End Function

    Public Sub NUMBER_GET_NORMALISED(ByRef Unit As Integer, ByRef Number As Double)
        While Math.Abs(Number) < 0.09995    'nem 0.99995 lett megadva, igy nem lesz 10:9,8765 arfolyam, hanem e helyett 1:0,9876, ami jobban megszokott.
            Unit *= 10
            Number *= 10
        End While

        While Math.Abs(Number) > 10.00005 And Unit > 1
            Unit /= 10
            Number /= 10
        End While
    End Sub

    Public Function TEXT_TRIM(MyText As String) As String
        Dim OUT As String = Trim(MyText)

        While Asc(Strings.Left(OUT, 1)) = 9
            OUT = Mid(OUT, 2)
        End While

        While Asc(Strings.Right(OUT, 1)) = 9
            OUT = Mid(OUT, 1, Len(OUT) - 1)
        End While

        Return OUT
    End Function

    Public Function TEXT_REMOVE_ALL_SPACES(ByVal MyText As String) As String
        Dim OUT As String = Replace(MyText, Chr(160), "")

        OUT = Replace(OUT, " ", "")

        TEXT_REMOVE_ALL_SPACES = OUT
    End Function

    Public Function ValDbl(ByVal MyStr As String) As Double
        Dim pPoint As Integer = InStrRev(MyStr, ".")
        Dim pComma As Integer = InStrRev(MyStr, ",")

        If pPoint > 0 And pComma > 0 Then
            If pPoint > pComma Then
                MyStr = Replace(MyStr, ",", "")
            Else
                MyStr = Replace(MyStr, ".", "")
                MyStr = Replace(MyStr, ",", ".")
            End If
        ElseIf pComma > 0 Then
            MyStr = Replace(MyStr, ",", ".")
        End If

        MyStr = TEXT_REMOVE_ALL_SPACES(MyStr)

        'MyStr = Replace(TEXT_REMOVE_ALL_SPACES(MyStr), ",", ".")
        ValDbl = Val(MyStr)
    End Function
    Public Function ValLong(ByVal MyStr As String) As Long
        MyStr = TEXT_REMOVE_ALL_SPACES(MyStr)
        Try
            ValLong = Val(MyStr)

        Catch ex As Exception
            ValLong = 0
        End Try
    End Function
    Public Function OBJECT_from_DBFORMAT(ByVal DBFormattedValue As String, ByVal FieldName As String, ByVal xtype_VB As String) As Object
        Dim OUT As Object

        Select Case xtype_VB
            Case "INT" : OUT = CType(Val(DBFormattedValue), Long)
            Case "" : OUT = CType(DBFormattedValue, String)
            Case "BIT" : OUT = CType(IIf(DBFormattedValue = "1", True, False), Boolean)
            Case "DATETIME" : OUT = CType(DBFORMAT_get_Date_From_DbStr(DBFormattedValue), DateTime)
            Case "FLOAT" : OUT = CType(Val(DBFormattedValue), Double)
            Case Else
                MsgBox(String.Format("FP.OBJECT_from_DBFORMAT: field {0} has an unknown xtype_VB ({1})", FieldName, xtype_VB))
                OUT = Nothing
        End Select

        OBJECT_from_DBFORMAT = OUT
    End Function

    Function TEXT_Is_HTML(Str As String) As Boolean
        Dim OUT As Boolean = False

        If Str > "" Then
            If InStr(Str, "<") > 0 Then
                If InStr(Str, "</") Then
                    OUT = True
                End If
            End If
        End If

        Return OUT
    End Function

    Function TEXT_Is_YES(Str As String) As Boolean
        Dim OUT As Boolean = False

        Str = Trim(Str).ToUpper

        If Str = "YES" Or Str = "1" Or Str = "IGEN" Or Str = "JA" Or Str = "TRUE" Then
            OUT = True
        End If

        Return OUT
    End Function

    Function TEXT_Is_NO(Str As String) As Boolean
        Dim OUT As Boolean = False

        Str = Trim(Str).ToUpper

        If Str = "NO" Or Str = "0" Or Str = "NEM" Or Str = "NEIN" Then
            OUT = True
        End If

        Return OUT
    End Function

    Function CHAR_Causes_Dirty(Char_Code As Integer) As Boolean
        'True-t ad vissza azoknal a karaktereknel, amelyek hatasara megvaltozik egy textbox tartalma
        Dim OUT As Boolean = True
        Dim ShiftPressed As Boolean = My.Computer.Keyboard.ShiftKeyDown
        Dim CtrlPressed As Boolean = My.Computer.Keyboard.CtrlKeyDown

        If CtrlPressed Then
            Select Case Char_Code
                Case Is < 32 'Ctrl onmagaban van lenyomva
                    OUT = False

                Case 86, 88 'Ctrl-V, Ctrl-X
                    OUT = True

                Case Else
                    OUT = False
            End Select
        Else
            Select Case Char_Code
                Case 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124 'F1-F12
                    OUT = False

                Case Else
                    OUT = True
            End Select
        End If

        'If OUT = True Then
        'MsgBox("+++")
        'End If

        Return OUT
    End Function

    Function TEXT_AND(ByVal a As String, ByVal b As String, Optional ByVal Op As String = "And")
        If Trim(a) > "" And Trim(b) > "" Then
            TEXT_AND = String.Format("({0}) {1} ({2})", a, Op, b)
        Else
            TEXT_AND = Trim(a) + Trim(b)
        End If
    End Function

    Function TEXT_GET_SEPARATED_STR_FROM_LIST(MyList As List(Of String), Separator As String) As String
        Dim OUT As String = ""
        Dim wSep As String = ""

        If Not (MyList Is Nothing) Then
            For Each a As String In MyList
                OUT = OUT + wSep + a
                wSep = Separator
            Next
        End If

        Return OUT
    End Function

    Function TEXT_IS_REMARK(MyText As String) As Boolean
        Dim OUT As Boolean = True

        MyText = Trim(nz(MyText, ""))

        If MyText > "" Then
            If Strings.Left(MyText, 1) <> "'" Then
                OUT = False
            End If
        End If

        Return OUT
    End Function

    Sub TV_NODE_GetAllChildren(ByVal MyNode As TreeNode, ByRef nodes As List(Of String))
        Dim i As Integer = 0

        Do While i < MyNode.Nodes.Count
            nodes.Add(MyNode.Nodes(i).Name)
            i += 1
        Loop

        i = 0
        Do While i < MyNode.Nodes.Count
            TV_NODE_GetAllChildren(MyNode.Nodes(i), nodes)
            i += 1
        Loop
    End Sub

    Sub TV_NODE_GetAllChildren(ByVal MyNode As TreeNode, ByRef nodes As List(Of TreeNode))
        Dim i As Integer = 0

        Do While i < MyNode.Nodes.Count
            nodes.Add(MyNode.Nodes(i))
            i += 1
        Loop

        i = 0
        Do While i < MyNode.Nodes.Count
            TV_NODE_GetAllChildren(MyNode.Nodes(i), nodes)
            i += 1
        Loop
    End Sub

    Function TV_NODE_Props_get(ByVal MyNode As TreeNode) As Struct_TreeNode_Props
        Dim OUT As New Struct_TreeNode_Props

        If Not (MyNode Is Nothing) Then
            With OUT
                .Name = MyNode.Name
                .Level = MyNode.Level

                If Not (MyNode.Parent Is Nothing) Then
                    .Parent_Name = MyNode.Parent.Name
                End If

                .IsExpanded = MyNode.IsExpanded
                .ImageKey = MyNode.ImageKey
                .BackColor = MyNode.BackColor
                .ForeColor = MyNode.ForeColor
                .Text = MyNode.Text
                .Tag = MyNode.Tag
                .ToolTipText = MyNode.ToolTipText
                .SelectedImageKey = MyNode.SelectedImageKey
                .NodeFont = MyNode.NodeFont
            End With
        End If

        Return OUT
    End Function

    Sub TV_NODE_MOVE(ByVal MyNode As TreeNode, ByVal ToParentNode As TreeNode, ByVal ToIndex As Integer, Optional ByRef DIC_Nodes As Dictionary(Of String, TreeNode) = Nothing)
        If Not (MyNode Is Nothing) Then

            Dim Lst_Children As New List(Of TreeNode)
            Dim Prop_Children As New Dictionary(Of String, Struct_TreeNode_Props)
            Dim MyNode_Props As Struct_TreeNode_Props = TV_NODE_Props_get(MyNode)
            Dim i As Integer = 0

            TV_NODE_GetAllChildren(MyNode, Lst_Children)

            i = 0
            Do While i < Lst_Children.Count
                Dim Childnode As TreeNode = Lst_Children(i)
                Prop_Children.Add(Childnode.Name, TV_NODE_Props_get(Childnode))

                i += 1
            Loop

            If Not (DIC_Nodes Is Nothing) Then
                For Each Key As String In Prop_Children.Keys
                    If DIC_Nodes.ContainsKey(Key) Then
                        DIC_Nodes.Remove(Key)
                    End If
                Next
            End If

            If Not (DIC_Nodes Is Nothing) Then
                If DIC_Nodes.Keys.Contains(MyNode.Name) Then
                    DIC_Nodes.Remove(MyNode.Name)
                End If
            End If

            If (MyNode.Parent Is Nothing) Then
                MyNode.TreeView.Nodes.Remove(MyNode)
            Else
                MyNode.Parent.Nodes.Remove(MyNode)
            End If

            If (DIC_Nodes Is Nothing) Then
                DIC_Nodes = New Dictionary(Of String, TreeNode)
            End If

            Dim NewNode As New TreeNode

            With NewNode
                .Name = MyNode_Props.Name
                .ImageKey = MyNode_Props.ImageKey
                .BackColor = MyNode_Props.BackColor
                .ForeColor = MyNode_Props.ForeColor
                .Text = MyNode_Props.Text
                .Tag = MyNode_Props.Tag
                .ToolTipText = MyNode_Props.ToolTipText
                .SelectedImageKey = MyNode_Props.SelectedImageKey
                .NodeFont = MyNode_Props.NodeFont
            End With

            If ToParentNode Is Nothing Then
                MyNode.TreeView.Nodes.Insert(ToIndex, MyNode)
            Else
                ToParentNode.Nodes.Insert(ToIndex, NewNode)
            End If
            DIC_Nodes.Add(NewNode.Name, NewNode)

            For Each Key As String In Prop_Children.Keys
                Dim NewChild As New TreeNode

                With Prop_Children(Key)
                    NewChild.Name = .Name

                    NewChild.ImageKey = .ImageKey
                    NewChild.BackColor = .BackColor
                    NewChild.ForeColor = .ForeColor
                    NewChild.Text = .Text
                    NewChild.Tag = .Tag
                    NewChild.ToolTipText = .ToolTipText
                    NewChild.SelectedImageKey = .SelectedImageKey
                    NewChild.NodeFont = .NodeFont

                    DIC_Nodes(.Parent_Name).Nodes.Add(NewChild)
                    DIC_Nodes.Add(NewChild.Name, NewChild)
                End With
            Next

            If MyNode_Props.IsExpanded = False Then
                NewNode.Collapse()
            Else
                NewNode.Expand()
            End If

            For Each Key As String In Prop_Children.Keys
                If Prop_Children(Key).IsExpanded = False Then
                    DIC_Nodes(Key).Collapse()
                Else
                    DIC_Nodes(Key).Expand()
                End If
            Next
        End If
    End Sub

    Function FIELD_PARENTFORM(ByVal c As Control) As Form
        Dim OUT As Form = c

        Dim DoIt As Boolean = True

        While DoIt
            If OUT Is Nothing Then
                DoIt = False
            Else
                If TypeOf (OUT) Is Form Then
                    DoIt = False
                Else
                    OUT = OUT.Parent
                End If
            End If
        End While

        FIELD_PARENTFORM = OUT
    End Function

    Function FIELD_IS_PARENT_OF(c_maybe_parent As Control, ByVal c As Control) As Boolean
        'Megadja, hogy c rajta van-e c_maybe_parent-en
        Dim OUT As Boolean = False

        If Not (c Is Nothing) Then
            Dim Current_c As Control = c.Parent

            Dim DoIt As Boolean = True

            While DoIt
                If Current_c Is Nothing Then
                    DoIt = False
                Else
                    If Current_c.Equals(c_maybe_parent) Then
                        DoIt = False
                        OUT = True
                    Else
                        Current_c = Current_c.Parent
                    End If
                End If
            End While
        End If

        Return OUT
    End Function

    Sub FIELD_VISIBLE_WITH_ENSURE(ByVal c As Control)
        If Not (c Is Nothing) Then
            Dim wc As Control = c
            Dim DoIt As Boolean = True

            If TypeOf (wc) Is TabPage Then
                If Not (wc.Parent Is Nothing) Then
                    CType(wc.Parent, TabControl).SelectTab(CType(wc, TabPage).Name)
                End If
            End If
            Do
                If TypeOf (wc.Parent) Is TabPage Then
                    Dim TC As TabControl = wc.Parent.Parent

                    If TC Is Nothing Then
                        DoIt = False
                    Else
                        TC.SelectedTab = wc.Parent
                        wc = wc.Parent
                        wc.Visible = True
                    End If
                ElseIf TypeOf (wc.Parent) Is TabControl Or TypeOf (wc.Parent) Is Panel Or TypeOf (wc.Parent) Is SplitContainer Then
                    wc = wc.Parent
                    wc.Visible = True
                Else
                    DoIt = False
                End If
            Loop While DoIt

            c.Visible = True
        End If
    End Sub

    Sub FIELD_VISIBLE(ByVal c As Control, ByVal MyVisible As Boolean)
        If Not (c Is Nothing) Then
            c.Visible = MyVisible

            'If c.Visible <> MyVisible Then
            '    c.Visible = MyVisible
            'Endif
            'End If
        End If
    End Sub

    Sub FIELD_LOCKED(ByRef e As System.Windows.Forms.KeyPressEventArgs)
        'Use this procedure in KeyPress event!
        Select Case Asc(e.KeyChar)
            Case 3 'Ctrl-C
                'Nothing to do

            Case Else
                e.Handled = True
        End Select
    End Sub

    Function FIELD_LOCKED_BUT_ENTER(ByRef e As System.Windows.Forms.KeyEventArgs) As Boolean
        Dim OUT As Boolean = False

        Select Case e.KeyCode
            Case Keys.Enter
                e.Handled = True
                OUT = True

            Case Else
                e.Handled = True
        End Select

        FIELD_LOCKED_BUT_ENTER = OUT
    End Function

    Sub FIELD_UCASE(ByVal MyTextBox As System.Windows.Forms.TextBox, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'Use this procedure in KeyPress event!
        If Char.IsLower(e.KeyChar) Then
            MyTextBox.SelectedText = Char.ToUpper(e.KeyChar)

            e.Handled = True
        End If
    End Sub

    Sub FIELD_SIMPLEDATE(ByVal MyTextBox As System.Windows.Forms.TextBox, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'Use this procedure in KeyPress event!
        If Char.IsLetterOrDigit(e.KeyChar) Then
            If Not IsNumeric(e.KeyChar) Then
                e.Handled = True
            Else
                If Not Date_IsSimpleDate(MyTextBox.Text + e.KeyChar, True) Then
                    e.Handled = True
                End If
            End If
        End If
    End Sub

    Sub FIELD_NUMERIC(ByVal e As System.Windows.Forms.KeyPressEventArgs, Optional ByVal FloatEnabled As Boolean = False, Optional ByVal NegativeEnabled As Boolean = False)
        'Use this procedure in KeyPress event!
        If Char.IsLetterOrDigit(e.KeyChar) Or e.KeyChar = "-" Or e.KeyChar = "," Or e.KeyChar = "." Then
            Dim EnabledChars As String = String.Format("{0}{1}+0123456789", IIf(FloatEnabled, ".,", ""), IIf(NegativeEnabled, "-", ""))
            If InStr(EnabledChars, e.KeyChar) = 0 Then
                e.Handled = True
            End If
        End If
    End Sub

    Sub FIELD_NUMERIC_UP(ByVal MyTextBox As System.Windows.Forms.TextBox, ByVal MyIncrement As Long, ByVal MinValue As Single, ByVal MaxValue As Single)
        If Val(MyTextBox.Text) + MyIncrement < MaxValue + 0.0005 Then
            MyTextBox.Text = Trim(Str(Val(MyTextBox.Text) + MyIncrement))
        End If
    End Sub

    Sub FIELD_NUMERIC_DOWN(ByVal MyTextBox As System.Windows.Forms.TextBox, ByVal MyIncrement As Long, ByVal MinValue As Single, ByVal MaxValue As Single)
        If Val(MyTextBox.Text) - MyIncrement > MinValue - 0.0005 Then
            MyTextBox.Text = Trim(Str(Val(MyTextBox.Text) - MyIncrement))
            If Math.Abs(Val(MyTextBox.Text) - MinValue) < 0.0005 Then
                MyTextBox.Text = MinValue.ToString
            End If
        End If
    End Sub

    Public Function FLOAT_IS_0(MyFloat As Double, Optional Round As Integer = 4) As Boolean
        Dim OUT As Boolean = False

        Select Case Round
            Case 0 : OUT = (Math.Abs(MyFloat) < 0.5)
            Case 1 : OUT = (Math.Abs(MyFloat) < 0.05)
            Case 2 : OUT = (Math.Abs(MyFloat) < 0.005)
            Case 3 : OUT = (Math.Abs(MyFloat) < 0.0005)
            Case 4 : OUT = (Math.Abs(MyFloat) < 0.00005)
            Case 5 : OUT = (Math.Abs(MyFloat) < 0.000005)
            Case 6 : OUT = (Math.Abs(MyFloat) < 0.0000005)
            Case Else
                DoErrorMsgBox_Without_SQL_Connection("FP_Globals.FLOAT_IS_0", 0, "Unknown value of parameter 'Round'")
                OUT = (Math.Abs(MyFloat) < 0.0005)
        End Select

        Return OUT
    End Function

    Sub CURSOR_SHOW_DEFAULT()
        Cursor.Current = Cursors.Default
        Cursor.Show()
    End Sub
    Sub CURSOR_SHOW_WAIT()
        Cursor.Current = Cursors.WaitCursor
        Cursor.Show()
    End Sub
    Sub CURSOR_HIDE_DEFAULT()
        Cursor.Current = Cursors.Default
        Cursor.Hide()
    End Sub
    Sub CURSOR_HIDE_WAIT()
        Cursor.Current = Cursors.WaitCursor
        Cursor.Hide()
    End Sub

    Sub CONTROL_DISPOSE(c As Control)
        If Not (c Is Nothing) Then
            If Not c.IsDisposed Then
                Try
                    c.Dispose()

                Catch ex As Exception
                    'Nothing to do
                End Try
            End If
        End If
    End Sub

    Function CONTROL_SET_VALUE(ByVal c As Control, ByVal Value As Object, Optional ByVal NothingIsOK As Boolean = False) As Boolean
        Dim OUT As Boolean = True

        If c Is Nothing Then
            If Not NothingIsOK Then
                MsgBox("FP_Globals: parameter 'c' is nothing.")
                OUT = False
            End If
        Else
            If TypeOf (c) Is Label Then
                c.Text = Value
            ElseIf TypeOf (c) Is TextBox Then
                c.Text = Value
            ElseIf TypeOf (c) Is RichTextBox Then
                CType(c, RichTextBox).Rtf = Value
            ElseIf TypeOf (c) Is ComboBox Then
                CType(c, ComboBox).SelectedValue = Value
            ElseIf TypeOf (c) Is CheckBox Then
                CType(c, CheckBox).Checked = (Val(Value) = 1)
            Else
                MsgBox(String.Format("FP_Globals.CONTROL_SET_VALUE: Control '{0}' has an unknown type.", c.Name))
                OUT = False
            End If
        End If

        CONTROL_SET_VALUE = OUT
    End Function

    Sub FOCUS_ON_IMMEDIATELY(ByRef c As Control)
        FIELD_VISIBLE_WITH_ENSURE(c)
        c.Focus()
        If c.Focused Then
            If TypeOf (c) Is System.Windows.Forms.TextBox Then
                CType(c, System.Windows.Forms.TextBox).SelectionStart = 0
                CType(c, System.Windows.Forms.TextBox).SelectionLength = CType(c, System.Windows.Forms.TextBox).Text.Length
            End If
        End If
    End Sub

    Function Date_getMaxDay(ByVal MyDate As DateTime) As Long
        Dim OUT As Long

        Select Case MyDate.Month
            Case 1, 3, 5, 7, 8, 10, 12
                OUT = 31

            Case 4, 6, 9, 11
                OUT = 30

            Case 2
                If (MyDate.Year Mod 4) = 0 Then
                    OUT = 29
                Else
                    OUT = 28
                End If
        End Select

        Date_getMaxDay = OUT
    End Function

    Function Date_IsSimpleDate(ByVal MyText As String, Optional ByVal PartOfDateOK As Boolean = False) As Boolean
        Dim OUT As Boolean = True
        Dim MyYear As Long
        Dim MyMonth As Long
        Dim MyDay As Long

        If OUT And Not IsNumeric(MyText) Then
            OUT = False
        End If

        If PartOfDateOK = False And Microsoft.VisualBasic.Len(MyText) <> 5 Then
            OUT = False
        End If

        If OUT And Microsoft.VisualBasic.Len(MyText) > 5 Then
            OUT = False
        End If

        MyYear = Val(Left(Trim(Str(Now.Year)), 3) + Trim(Str(Val(Mid(MyText, 1, 1)))))
        MyMonth = Val(Mid(MyText, 2, 2))
        MyDay = Val(Mid(MyText, 4, 2))

        If OUT And Microsoft.VisualBasic.Len(MyText) >= 3 Then
            If MyMonth < 1 Or MyMonth > 12 Then
                OUT = False
            End If
        End If

        If OUT And Microsoft.VisualBasic.Len(MyText) >= 5 Then
            If MyDay < 1 Or MyDay > Date_getMaxDay(DateSerial(MyYear, MyMonth, 1)) Then
                OUT = False
            End If
        End If

        Date_IsSimpleDate = OUT
    End Function

    Function SQL_BIND_DD(ByRef sda As Data.SqlClient.SqlDataAdapter, ByRef dd As ComboBox, ByVal val As String, ByVal disp As String) As System.Data.DataTable
        Dim ds As New Data.DataSet
        sda.Fill(ds, "RESULT")
        dd.DataSource = ds.Tables("RESULT")
        dd.ValueMember = val
        dd.DisplayMember = disp
        dd.Update()
        '
        Return ds.Tables("RESULT")
    End Function

    Public Function SQLDate(ByVal MyDate As Date, Optional ByVal set2359 As Boolean = False, Optional DateFormat As ENUM_StrDate_Foramt = ENUM_StrDate_Foramt.FOR_SQL_SERVER) As String
        Dim OUT As String = ""

        If MyDate = NULLDATE Then
            OUT = "NULL"
        Else
            If set2359 Then
                MyDate = New DateTime(MyDate.Year, MyDate.Month, MyDate.Day, 23, 59, 59)
            End If

            Select Case DateFormat
                Case ENUM_StrDate_Foramt.FOR_SQL_SERVER
                    Dim MyDate_STR As String = String.Format("{0}-{1}-{2} {3}:{4}:{5}",
                                                                 DateAndTime.Year(MyDate).ToString,
                                                                 DateAndTime.Month(MyDate).ToString,
                                                                 DateAndTime.Day(MyDate).ToString,
                                                                 DateAndTime.Hour(MyDate).ToString,
                                                                 DateAndTime.Minute(MyDate),
                                                                 DateAndTime.Second(MyDate).ToString
                                                                 )

                    OUT = "CONVERT(datetime, '" + MyDate_STR + "', 20)"
                    'SQLDatum$ = Left$(SQLDatum$, 29) + Replace(SQLDatum$, ".", ":", 30, 2)

                Case ENUM_StrDate_Foramt.FOR_DOTNET
                    OUT = String.Format("{0}-{1}-{2}T{3}:{4}:{5}", MyDate.Year.ToString,
                                                                   Strings.Right("00" + MyDate.Month.ToString, 2),
                                                                   Strings.Right("00" + MyDate.Day.ToString, 2),
                                                                   Strings.Right("00" + MyDate.Hour.ToString, 2),
                                                                   Strings.Right("00" + MyDate.Minute.ToString, 2),
                                                                   Strings.Right("00" + MyDate.Second.ToString, 2)
                                                                   )

                Case Else
                    OUT = "???"
            End Select
        End If

        SQLDate = OUT
    End Function

    Public Function SQLStr(MySTR) As String
        Dim OUT As String = nz(MySTR, "")

        OUT = Replace(MySTR, "'", "''")

        Return OUT
    End Function

    Sub NAVIGATION_QUIT()
        Application.Exit()
    End Sub

    Function Barcode_IsReaded(ByVal MyString As String, Optional ByRef MyStringWithoutBarcodeEND As String = "") As Boolean
        Dim OUT As Boolean

        If Microsoft.VisualBasic.Right(MyString, 5) = "#END#" Then
            MyStringWithoutBarcodeEND = Microsoft.VisualBasic.Left(MyString, Microsoft.VisualBasic.Len(MyString) - 5)

            OUT = True
        Else
            OUT = False
        End If

        Barcode_IsReaded = OUT
    End Function

    Public Function DataGrid_Search(ByVal dgw As System.Windows.Forms.DataGrid, ByVal MyColumnIndex As Long, ByVal ValueToSearch As String, Optional ByVal FromRow As Long = 0, Optional ByVal DataGrid_CountOfRows As Long = -1) As Long
        Dim OUT As Long = -1
        Dim pRow As Long
        Dim NoErr As Boolean = True

        ValueToSearch = ValueToSearch.Trim.ToLower
        pRow = FromRow
        OUT = -1
        Try
            Do While NoErr And OUT = -1
                If DataGrid_CountOfRows <> -1 And pRow > DataGrid_CountOfRows Then
                    NoErr = False
                Else
                    Try
                        If dgw.Item(pRow, MyColumnIndex).ToString.Trim.ToLower = ValueToSearch Then
                            OUT = pRow
                        End If

                    Catch ex As Exception
                        NoErr = False
                    End Try
                End If

                pRow = pRow + 1
            Loop

        Catch ex As Exception
            OUT = -1
        End Try

        DataGrid_Search = OUT

    End Function

    Public Function IsNull(ByVal MyValue As Object) As Boolean
        Dim OUT As Boolean = False
        Try
            If IsNothing(MyValue) Then
                OUT = True
            Else
                If IsDBNull(MyValue) Then
                    OUT = True
                Else
                    If MyValue.GetType().ToString().ToUpper() = "SYSTEM.STRING" Then
                        OUT = (MyValue = "##Nothing##")
                    End If
                End If
            End If
        Catch ex As Exception
            DoErrorMsgBox_Without_SQL_Connection("FP_Globals.Isnull", Err.Number, Err.Description)
        End Try
        Return OUT
    End Function

    Public Function KommaToPunkt(ByVal MyText As String) As String
        Dim OUT As String = String.Empty
        Dim w As Integer
        Try
            w = InStr(MyText, ",")
            Do While w > 0
                Mid(MyText, w, 1) = "."
                w = InStr(MyText, ",")
            Loop
            OUT = MyText
        Catch ex As Exception
        End Try
        Return OUT
    End Function

    Function getDirectoryFromFullName(ByVal MyName As String) As String
        Dim OUT As String = ""

        Dim w As Integer

        MyName = Trim(MyName)
        For w = Len(MyName) To 1 Step -1
            If Mid(MyName, w, 1) = "\" Then
                OUT = Mid(MyName, 1, w)
                Exit For
            End If
        Next w

        getDirectoryFromFullName$ = OUT
    End Function

    Public Sub ReleaseObject(ByRef obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub

    Function getFileExtension(FullName As String, Optional WithPoint As Boolean = False) As String
        Dim OUT As String = ""
        Dim p As Integer = InStrRev(FullName, ".")

        If p = 0 Then
            OUT = ""
        Else
            If WithPoint Then
                OUT = Mid(FullName, p)
            Else
                OUT = Mid(FullName, p + 1)
            End If
        End If

        Return OUT
    End Function

    Function getFileName_without_Extension(FullName As String) As String
        Dim OUT As String = getFileNameFromFullName(FullName)
        Dim p As Integer = InStrRev(OUT, ".")

        If p > 0 Then
            OUT = Mid(OUT, 1, p - 1)

        End If

        Return OUT
    End Function

    Function getFileNameFromFullName(ByVal FullName As String) As String
        Dim OUT As String = Trim(FullName)

        Dim p As Integer = InStrRev(OUT, "\")
        If p > 0 Then
            OUT = Mid(OUT, p + 1)
        End If

        p = InStrRev(OUT, "/")
        If p > 0 Then
            OUT = Mid(OUT, p + 1)
        End If

        getFileNameFromFullName = OUT
    End Function

    Public Function Path_And_FileName(ByVal Path As String, ByVal FileName As String, Optional ByVal Separator As String = "\") As String
        Dim OUT As String = FileName

        If Path > "" Then
            If Right(Path, 1) = Separator Then
                OUT = Path + OUT
            Else
                OUT = Path + Separator + OUT
            End If
        End If

        Return OUT
    End Function

    Public Function DATE2359(ByVal MyDate As DateTime) As DateTime
        Return New DateTime(MyDate.Year, MyDate.Month, MyDate.Day, 23, 59, 59)
    End Function

    Public Function FN_PRINTED_MONEY_FIX_LENGTH(Number As Double, Digits As Integer, FixLength As Integer) As String
        Dim OUT As String = Format(Number, "N" & Digits)

        FixLength = FixLength * 2

        If Len(OUT) <= FixLength Then
            Dim p As Integer = 1
            Do While p <= Len(OUT)
                Dim Dig As String = Mid(OUT, p, 1)
                If Dig = " " Or Dig = "," Or Dig = "." Then
                    FixLength = FixLength - 1
                Else
                    FixLength = FixLength - 2
                End If

                p += 1
            Loop

            If FixLength > 0 Then
                OUT = Space(FixLength) + OUT
            End If
        End If

        Return OUT

    End Function

    Public Function MSWORD_CELL_REMOVE_SYS_CHARS(MyStr As String) As String
        Dim OUT As String = MyStr
        If OUT = String.Empty Then
            'nothing to do
        ElseIf OUT = Nothing Then
            OUT = ""
        Else
            If OUT.Contains(Chr(7)) Then
                OUT = Replace(MyStr, Chr(7), "")
            End If
            If OUT.Contains(Chr(11)) Then
                OUT = Replace(MyStr, Chr(11), "")
            End If

            If Mid(OUT, Len(OUT), 1) = Chr(13) Then
                OUT = Mid(OUT, 1, Len(OUT) - 1)
            End If
        End If
        Return OUT
    End Function

    Public Function FPc_Set_Text_From_Selected_ID_REFRESH_ONLY(FPc As FP_Control, LookUp_TableName As String, LookUp_FieldName As String) As Boolean
        Dim OUT As Boolean = True

        If Not (FPc Is Nothing) Then
            If Not (FPc.c Is Nothing) Then
                If Not TypeOf (FPc.c) Is ComboBox Then
                    If Not (FPc.FP Is Nothing) Then
                        Dim Current_Selected_ID = FPc.Selected_ID
                        Dim New_Text As String = ""

                        If Current_Selected_ID <> 0 Then
                            Dim MySQL As String = String.Format("SELECT {0} FROM {1} WHERE ID = {2}", LookUp_FieldName, LookUp_TableName, FPc.Selected_ID)
                            Dim DRow As DataRow = FPc.FP.FPf.FPApp.DC.Qdf_get_DataRow(MySQL)

                            If DRow Is Nothing Then
                                New_Text = ""
                            Else
                                New_Text = DRow.Item(LookUp_FieldName)
                            End If
                        End If

                        FPc.FP.P_DATA_Binded_ByUser = False
                        FPc.c.Text = New_Text
                        FPc.FP.P_DATA_Binded_ByUser = True
                    End If
                End If
            End If
        End If

        Return OUT
    End Function


    Public Function FPc_Set_Text_From_Selected_ID_And_DIRTY_SET(FPc As FP_Control, LookUp_TableName As String, LookUp_FieldName As String) As Boolean
        Dim OUT As Boolean = True

        If Not (FPc Is Nothing) Then
            If Not (FPc.c Is Nothing) Then
                If Not (FPc.FP Is Nothing) Then
                    Dim Current_Selected_ID = FPc.Selected_ID
                    If Current_Selected_ID <> 0 Then
                        Dim MySQL As String = String.Format("SELECT {0} FROM {1} WHERE ID = {2}", LookUp_FieldName, LookUp_TableName, FPc.Selected_ID)
                        Dim DRow As DataRow = FPc.FP.FPf.FPApp.DC.Qdf_get_DataRow(MySQL)

                        If OUT = True Then
                            OUT = Not (FPc.P.Locked)
                        End If

                        If OUT = True Then
                            OUT = FPc.FP.FORM_DIRTY_SET
                        End If

                        If OUT = True Then
                            Try
                                If DRow Is Nothing Then
                                    FPc.P_VALUE = ""
                                    FPc.Selected_ID = 0
                                Else
                                    FPc.P_VALUE = DRow.Item(LookUp_FieldName)
                                    FPc.Selected_ID = Current_Selected_ID
                                End If

                            Catch ex As Exception
                                OUT = False
                            End Try
                        End If
                    End If
                End If
            End If
        End If

        Return OUT
    End Function

    Public Function INV_Types_IsOutgoing(Inv_Type_ID As ENUM_INV_Types) As Boolean
        Return (Inv_Type_ID = ENUM_INV_Types.OUT Or
          Inv_Type_ID = ENUM_INV_Types.OUT_CREDITNOTE Or
          Inv_Type_ID = ENUM_INV_Types.OUT_CREDITNOTE_CANCEL Or
          Inv_Type_ID = ENUM_INV_Types.OUT_CORRECTION Or
          Inv_Type_ID = ENUM_INV_Types.ADVANCE_PAYMENT Or
          Inv_Type_ID = ENUM_INV_Types.RETURNS Or
          Inv_Type_ID = ENUM_INV_Types.DEPOSIT Or
          Inv_Type_ID = ENUM_INV_Types.OUT_CORRECTION_DOC) Or
          Inv_Type_ID = ENUM_INV_Types.ACC_DOC_OUT
    End Function

    Public Function INV_Types_IsIncoming(Inv_Type_ID As ENUM_INV_Types) As Boolean
        Return (Inv_Type_ID = ENUM_INV_Types.[IN] Or
          Inv_Type_ID = ENUM_INV_Types.IN_CREDITNOTE Or
          Inv_Type_ID = ENUM_INV_Types.IN_CORRECTION Or
          Inv_Type_ID = ENUM_INV_Types.ACC_DOC_IN Or
          Inv_Type_ID = ENUM_INV_Types.IN_CORRECTION_DOC)
    End Function

    Public Function Users_Email(Users_ID) As String
        Dim OUT As String = ""
        Dim MySQL As String = String.Format("SELECT dbo.FN_USER_EMAIL_2({0}) EMAIL", Users_ID)
        Dim DRow As DataRow = gl_FPApp.DC.Qdf_get_DataRow(MySQL)

        If Not (DRow Is Nothing) Then
            OUT = DRow!EMAIL
        End If

        Return OUT
    End Function
#Region "JSON SELECT CONTROL DEF"
    Public Structure Stru_Select_Control_Field_Prop
        Dim FieldName As String
        Dim FieldLen As Integer
        Dim HeaderTexts As Dictionary(Of String, Stru_Select_Control_Field_Headers)
        Dim Visible As Boolean
    End Structure

    Public Structure Stru_Select_Control_Field_Headers
        Dim Fieldname As String
        Dim Language As String
        Dim HeaderText As String
    End Structure

    Private Function JSon_Select_Control_Load_Simple_Params(JT As KeyValuePair(Of String, Linq.JToken), ByRef DIC_Params As Dictionary(Of String, String)) As Boolean
        Dim OUT As Boolean = True
        If JT.Value.Type = Linq.JTokenType.String Then
            DIC_Params.Add(JT.Key, JT.Value)
        End If
        Return OUT
    End Function
    Public Function JSon_Select_Control_Load_Params(Control_Definition As String,
                                                    ByRef DIC_Params As Dictionary(Of String, String),
                                                    ByRef Dic_Fields As Dictionary(Of String, Stru_Select_Control_Field_Prop),
                                                    Optional ByRef GridWidth As Integer = 0) As Boolean
        Dim OUT As Boolean = True
        Dim read As JObject
        Dim Dic_Header_Names As Dictionary(Of String, Stru_Select_Control_Field_Headers)
        Try
            read = JObject.Parse(Control_Definition)
        Catch ex As Exception
            read = Nothing
        End Try
        If Not read Is Nothing Then
            For Each JT As KeyValuePair(Of String, JToken) In read
                JSon_Select_Control_Load_Simple_Params(JT, DIC_Params)
            Next

            GridWidth = 0

            Dim def As JObject
            If Not read.Item("Field_Def")("items").First Is Nothing Then
                Dic_Fields = New Dictionary(Of String, Stru_Select_Control_Field_Prop)
                Dim FD As New Stru_Select_Control_Field_Prop
                def = read.Item("Field_Def")("items").First
                Do
                    With FD
                        .FieldName = def.Item("FieldName").ToString.ToUpper
                        .FieldLen = def.Item("FieldLen")
                        .Visible = def.Item("Visible")
                        GridWidth += .FieldLen + 10

                        Dim HDef As JObject
                        Dim HT As Stru_Select_Control_Field_Headers
                        Dic_Header_Names = New Dictionary(Of String, Stru_Select_Control_Field_Headers)
                        HDef = def.Item("Headers")("items").First
                        Do
                            With HT
                                .Fieldname = FD.FieldName
                                .Language = HDef.Item("Lang")
                                .HeaderText = HDef.Item("HeaderText")
                                Dic_Header_Names.Add(.Language, HT)
                            End With
                            HDef = HDef.Next
                        Loop Until HDef Is Nothing

                        .HeaderTexts = Dic_Header_Names
                        Dic_Fields.Add(.FieldName, FD)
                    End With
                    def = def.Next
                Loop Until def Is Nothing
            End If
            Return OUT
        End If
    End Function

#End Region

    Public ReadOnly Property Organisation_Handling As Boolean
        Get
            Dim OUT As Boolean = True
            Dim MyKey As String = "ORGANISATION_HANDLING"
            If Not gl_FPApp.NEW_DEVELOPMENT_PARAMS_JSON_IsInstalled(MyKey) Then
                OUT = False
            End If
            If Not gl_FPApp.Installed_Products_Exists(MyKey) Then
                OUT = False
            End If
            Return OUT
        End Get
    End Property

    Public ReadOnly Property Failover_Handling As Boolean
        Get
            Dim OUT As Boolean = True
            Dim MyKey As String = "FAILOVER_HANDLING"
            If Not gl_FPApp.NEW_DEVELOPMENT_PARAMS_JSON_IsInstalled(MyKey) Then
                OUT = False
            End If
            If Not gl_FPApp.Installed_Products_Exists(MyKey) Then
                OUT = False
            End If
            Return OUT
        End Get
    End Property

    Public ReadOnly Property CrossDock_Handling As Boolean
        Get
            Dim OUT As Boolean = True
            Dim MyKey As String = "CROSSDOCK_HANDLING"
            If Not gl_FPApp.NEW_DEVELOPMENT_PARAMS_JSON_IsInstalled(MyKey) Then
                OUT = False
            End If
            If Not gl_FPApp.Installed_Products_Exists(MyKey) Then
                OUT = False
            End If
            Return OUT
        End Get
    End Property

End Module
