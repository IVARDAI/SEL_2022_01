Public Class FP_ADDRESS_Panel
    Public Structure Struct_FP_ADDRESS_Panel_Params
        Dim FP As FP

        Dim Address_Panel As Panel
        Dim FPc_Country As FP_Control           'Legyen label-je Country_Label neven!
        Dim FPc_State As FP_Control             'Legyen label-je Country_Label neven!
        Dim FPc_ZIP As FP_Control               'Legyen label-je ZIP_Label neven!
        Dim FPc_District As FP_Control          'Legyen label-je District_Label neven!
        Dim FPc_City As FP_Control              'Legyen label-je City_Label neven!
        Dim FPc_Addr As FP_Control              'Legyen label-je Addr_Label neven!
        Dim FPc_Addr_ps_type As FP_Control      'Legyen label-je Addr_ps_type_Label neven!
        Dim FPc_Addr_housenr As FP_Control      'Legyen label-je Addr_housenr_Label neven!
        Dim FPc_Addr_building As FP_Control     'Legyen label-je Addr_building_Label neven!
        Dim FPc_Addr_stairway As FP_Control     'Legyen label-je Addr_stairway_Label neven!
        Dim FPc_Addr_floor As FP_Control        'Legyen label-je Addr_floor_Label neven!
        Dim FPc_Addr_door As FP_Control         'Legyen label-je Addr_door_Label neven!

        Dim FPc_Addr_Type As FP_Control
    End Structure

    Public WithEvents FP As FP

    Public WithEvents Address_Panel As Panel
    Public FPc_Country As FP_Control = Nothing             'Legyen label-je Country_Label neven!
    Public FPc_ZIP As FP_Control = Nothing                 'Legyen label-je ZIP_Label neven!
    Public FPc_State As FP_Control = Nothing               'Legyen label-je ZIP_Label neven!
    Public FPc_District As FP_Control = Nothing            'Legyen label-je District_Label neven!
    Public FPc_City As FP_Control = Nothing                'Legyen label-je City_Label neven!
    Public FPc_Addr As FP_Control = Nothing                'Legyen label-je Addr_Label neven!
    Public FPc_Addr_ps_type As FP_Control = Nothing        'Legyen label-je Addr_ps_type_Label neven!
    Public FPc_Addr_housenr As FP_Control = Nothing        'Legyen label-je Addr_housenr_Label neven!
    Public FPc_Addr_building As FP_Control = Nothing       'Legyen label-je Addr_building_Label neven!
    Public FPc_Addr_stairway As FP_Control = Nothing       'Legyen label-je Addr_stairway_Label neven!
    Public FPc_Addr_floor As FP_Control = Nothing          'Legyen label-je Addr_floor_Label neven!
    Public FPc_Addr_door As FP_Control = Nothing           'Legyen label-je Addr_door_Label neven!
    Public WithEvents FPc_Addr_Type As FP_Control = Nothing

    Public DIC_SETTINGS As Dictionary(Of String, String)

    Private Disposed As Boolean = False

    Sub New(P As Struct_FP_ADDRESS_Panel_Params)
        With P
            FP = .FP

            Address_Panel = .Address_Panel

            FPc_Country = .FPc_Country
            FPc_ZIP = .FPc_ZIP
            FPc_State = .FPc_State
            FPc_District = .FPc_District
            FPc_City = .FPc_City
            FPc_Addr = .FPc_Addr
            FPc_Addr_ps_type = .FPc_Addr_ps_type
            FPc_Addr_housenr = .FPc_Addr_housenr
            FPc_Addr_building = .FPc_Addr_building
            FPc_Addr_stairway = .FPc_Addr_stairway
            FPc_Addr_floor = .FPc_Addr_floor
            FPc_Addr_door = .FPc_Addr_door

            FPc_Addr_Type = .FPc_Addr_Type
        End With

        Dim SETTINGS_Key As String = "NATIONAL_SETTINGS"
        Dim FixText_SETTINGS As String = gl_FPApp.getFixText(SETTINGS_Key)

        gl_FPApp.FIXTEXT_SPLIT_PARAMS(FixText_SETTINGS, DIC_SETTINGS)
    End Sub


    Public Sub Dispose()
        If Disposed = False Then
            Disposed = True

            Address_Panel = Nothing

            FPc_Country = Nothing
            FPc_ZIP = Nothing
            FPc_District = Nothing
            FPc_City = Nothing
            FPc_Addr = Nothing
            FPc_Addr_ps_type = Nothing
            FPc_Addr_housenr = Nothing
            FPc_Addr_building = Nothing
            FPc_Addr_stairway = Nothing
            FPc_Addr_floor = Nothing
            FPc_Addr_door = Nothing

            FPc_Addr_Type = Nothing

            FP = Nothing
        End If
    End Sub

    Public Sub SET_LAYOUT_HIDE()
        Address_Panel.Visible = False
    End Sub

    Private ReadOnly Property P_Spec_Addr_Default_Value As Boolean
        Get
            Dim OUT As Boolean = False

            OUT = (gl_FPApp.FIXTEXT_getParam("SPEC_ADDR_DEFAULT_VALUE", DIC_SETTINGS) = "1")

            Return OUT
        End Get
    End Property

    Private Sub SET_LAYOUT_WIDTH_SIMPLE()
        'Layout-tol nem fuggo mezok lathatova tetele
        FPc_Country.P_VISIBLE = True
        FPc_ZIP.P_VISIBLE = True
        FPc_City.P_VISIBLE = True
        FPc_Addr.P_VISIBLE = True
        FPc_State.P_VISIBLE = True

        'Layout-tol fuggo mezok lathatosaganak beallitasa
        FPc_District.P_VISIBLE = False

        With FP.FPf
            FPc_Country.c_Label.Left = 0
            FPc_Country.c_Label.Top = 0
            FPc_Country.c_Label.Width = 50
            .ARRANGE_ON_RIGHT_TOP(FPc_State.c_Label, FPc_Country.c_Label, 0)
            .SIZE_WIDTH_TO(FPc_State.c_Label, Address_Panel, , , 15)
            .ARRANGE_ON_RIGHT_TOP(FPc_ZIP.c_Label, FPc_State.c_Label, 0)
            .SIZE_WIDTH_TO(FPc_ZIP.c_Label, Address_Panel, , , 15)

            FPc_District.P_VISIBLE = False

            .ARRANGE_ON_RIGHT_TOP(FPc_City.c_Label, FPc_ZIP.c_Label, 0)
            .SIZE_WIDTH_BETWEEN(FPc_City.c_Label, FPc_ZIP.c_Label, Address_Panel)

            FPc_Country.c.Left = FPc_Country.c_Label.Left
            FPc_Country.c.Width = FPc_Country.c_Label.Width

            FPc_State.c.Left = FPc_State.c_Label.Left
            FPc_State.c.Width = FPc_State.c_Label.Width

            FPc_ZIP.c.Left = FPc_ZIP.c_Label.Left
            FPc_ZIP.c.Width = FPc_ZIP.c_Label.Width

            FPc_City.c.Left = FPc_City.c_Label.Left
            FPc_City.c.Width = FPc_City.c_Label.Width

            FPc_Addr.c.Left = 0
            FPc_Addr.c.Width = Address_Panel.Width

            FPc_Addr.c_Label.Left = 0
            FPc_Addr.c_Label.Width = FPc_Addr.c.Width
        End With

        FPc_Addr_ps_type.P_VISIBLE = False
        FPc_Addr_housenr.P_VISIBLE = False
        FPc_Addr_building.P_VISIBLE = False
        FPc_Addr_stairway.P_VISIBLE = False
        FPc_Addr_floor.P_VISIBLE = False
        FPc_Addr_door.P_VISIBLE = False

        If FPc_Addr_Type.P_VALUE <> True Then
            Dim DBinded As Boolean = FP.P_DATA_Binded_ByUser

            FP.P_DATA_Binded_ByUser = False
            FPc_Addr_Type.P_VALUE = True
            FP.P_DATA_Binded_ByUser = DBinded
        End If
    End Sub

    Private Sub SET_LAYOUT_WIDTH_NORMAL()
        'Layout-tol nem fuggo mezok lathatova tetele
        FPc_Country.P_VISIBLE = True
        FPc_ZIP.P_VISIBLE = True
        FPc_City.P_VISIBLE = True
        FPc_Addr.P_VISIBLE = True
        FPc_State.P_VISIBLE = True

        'Layout-tol fuggo mezok lathatosaganak beallitasa
        FPc_District.P_VISIBLE = True

        FPc_Addr_ps_type.P_VISIBLE = True
        FPc_Addr_housenr.P_VISIBLE = True
        FPc_Addr_building.P_VISIBLE = True
        FPc_Addr_stairway.P_VISIBLE = True
        FPc_Addr_floor.P_VISIBLE = True
        FPc_Addr_door.P_VISIBLE = True

        With FP.FPf
            FPc_Country.c_Label.Left = 0
            FPc_Country.c_Label.Top = 0
            FPc_Country.c_Label.Width = 50
            .ARRANGE_ON_RIGHT_TOP(FPc_State.c_Label, FPc_Country.c_Label, 0)
            .SIZE_WIDTH_TO(FPc_State.c_Label, Address_Panel, , , 15)
            .ARRANGE_ON_RIGHT_TOP(FPc_ZIP.c_Label, FPc_State.c_Label, 0)
            .SIZE_WIDTH_TO(FPc_ZIP.c_Label, Address_Panel, , , 15)
            .ARRANGE_ON_RIGHT_TOP(FPc_District.c_Label, FPc_ZIP.c_Label, 0)
            .SIZE_WIDTH_TO(FPc_District.c_Label, Address_Panel, , , 15)
            .ARRANGE_ON_RIGHT_TOP(FPc_City.c_Label, FPc_District.c_Label, 0)
            .SIZE_WIDTH_BETWEEN(FPc_City.c_Label, FPc_District.c_Label, Address_Panel)

            FPc_Country.c.Left = FPc_Country.c_Label.Left
            FPc_Country.c.Width = FPc_Country.c_Label.Width

            FPc_State.c.Left = FPc_State.c_Label.Left
            FPc_State.c.Width = FPc_State.c_Label.Width

            FPc_ZIP.c.Left = FPc_ZIP.c_Label.Left
            FPc_ZIP.c.Width = FPc_ZIP.c_Label.Width

            FPc_District.c.Left = FPc_District.c_Label.Left
            FPc_District.c.Width = FPc_District.c_Label.Width

            FPc_City.c.Left = FPc_City.c_Label.Left
            FPc_City.c.Width = FPc_City.c_Label.Width

            FPc_Addr.c.Left = 0
            FPc_Addr.c.Width = FPc_Country.c.Width + FPc_State.c.Width + FPc_ZIP.c.Width + FPc_District.c.Width

            .SIZE_WIDTH_TO(FPc_Addr_ps_type.c, FPc_City.c, , , 16)
            .ARRANGE_ON_RIGHT(FPc_Addr_ps_type.c, FPc_Addr.c, 0)
            .SIZE_WIDTH_TO(FPc_Addr_housenr.c, FPc_City.c, , , 17)
            .ARRANGE_ON_RIGHT(FPc_Addr_housenr.c, FPc_Addr_ps_type.c, 0)
            FPc_Addr_housenr.c.Width = FPc_Addr_ps_type.c.Width
            .ARRANGE_ON_RIGHT(FPc_Addr_building.c, FPc_Addr_housenr.c, 0)
            FPc_Addr_building.c.Width = FPc_Addr_ps_type.c.Width
            .ARRANGE_ON_RIGHT(FPc_Addr_stairway.c, FPc_Addr_building.c, 0)
            FPc_Addr_stairway.c.Width = FPc_Addr_ps_type.c.Width
            .ARRANGE_ON_RIGHT(FPc_Addr_floor.c, FPc_Addr_stairway.c, 0)
            FPc_Addr_floor.c.Width = FPc_Addr_ps_type.c.Width
            .ARRANGE_ON_RIGHT(FPc_Addr_door.c, FPc_Addr_floor.c, 0)
            .SIZE_WIDTH_BETWEEN(FPc_Addr_door.c, FPc_Addr_floor.c, Address_Panel)

            FPc_Addr.c_Label.Left = 0
            FPc_Addr.c_Label.Width = FPc_Addr.c.Width

            FPc_Addr_ps_type.c_Label.Left = FPc_Addr_ps_type.c.Left
            FPc_Addr_ps_type.c_Label.Width = FPc_Addr_ps_type.c.Width

            FPc_Addr_housenr.c_Label.Left = FPc_Addr_housenr.c.Left
            FPc_Addr_housenr.c_Label.Width = FPc_Addr_housenr.c.Width

            FPc_Addr_building.c_Label.Left = FPc_Addr_building.c.Left
            FPc_Addr_building.c_Label.Width = FPc_Addr_building.c.Width

            FPc_Addr_stairway.c_Label.Left = FPc_Addr_stairway.c.Left
            FPc_Addr_stairway.c_Label.Width = FPc_Addr_stairway.c.Width

            FPc_Addr_floor.c_Label.Left = FPc_Addr_floor.c.Left
            FPc_Addr_floor.c_Label.Width = FPc_Addr_floor.c.Width

            FPc_Addr_door.c_Label.Left = FPc_Addr_door.c.Left
            FPc_Addr_door.c_Label.Width = FPc_Addr_door.c.Width

            FPc_Addr_Type.c.Left = 0
            .ARRANGE_ON_RIGHT(FPc_Addr_Type.c_Label, FPc_Addr_Type.c)
            .SIZE_WIDTH_BETWEEN(FPc_Addr_Type.c_Label, FPc_Addr_Type.c, Address_Panel)
        End With

        If FPc_Addr_Type.P_VALUE <> False Then
            Dim DBinded As Boolean = FP.P_DATA_Binded_ByUser

            FP.P_DATA_Binded_ByUser = False
            FPc_Addr_Type.P_VALUE = False
            FP.P_DATA_Binded_ByUser = DBinded
        End If
    End Sub

    Private Sub SET_LAYOUT_HEIGHT()
        If Disposed = False Then
            With FP.FPf
                FPc_Country.c_Label.Left = 0
                FPc_Country.c_Label.Top = 0
                FPc_ZIP.c_Label.Top = 0
                FPc_District.c_Label.Top = 0
                FPc_City.c_Label.Top = 0
                FPc_State.c_Label.Top = 0

                .ARRANGE_ON_BOTTOM(FPc_Country.c, FPc_Country.c_Label, 0)
                .ARRANGE_TOPS(FPc_State.c, FPc_Country.c)
                .ARRANGE_TOPS(FPc_ZIP.c, FPc_Country.c)
                .ARRANGE_TOPS(FPc_District.c, FPc_Country.c)
                .ARRANGE_TOPS(FPc_City.c, FPc_Country.c)

                .ARRANGE_ON_BOTTOM(FPc_Addr.c, FPc_Country.c, -1)
                .ARRANGE_TOPS(FPc_Addr_ps_type.c, FPc_Addr.c)
                .ARRANGE_TOPS(FPc_Addr_housenr.c, FPc_Addr.c)
                .ARRANGE_TOPS(FPc_Addr_building.c, FPc_Addr.c)
                .ARRANGE_TOPS(FPc_Addr_stairway.c, FPc_Addr.c)
                .ARRANGE_TOPS(FPc_Addr_floor.c, FPc_Addr.c)
                .ARRANGE_TOPS(FPc_Addr_door.c, FPc_Addr.c)

                .ARRANGE_ON_BOTTOM(FPc_Addr.c_Label, FPc_Addr.c, 0)
                .ARRANGE_TOPS(FPc_Addr_ps_type.c_Label, FPc_Addr.c_Label)
                .ARRANGE_TOPS(FPc_Addr_housenr.c_Label, FPc_Addr.c_Label)
                .ARRANGE_TOPS(FPc_Addr_building.c_Label, FPc_Addr.c_Label)
                .ARRANGE_TOPS(FPc_Addr_stairway.c_Label, FPc_Addr.c_Label)
                .ARRANGE_TOPS(FPc_Addr_floor.c_Label, FPc_Addr.c_Label)
                .ARRANGE_TOPS(FPc_Addr_door.c_Label, FPc_Addr.c_Label)

                .ARRANGE_ON_BOTTOM(FPc_Addr_Type.c, FPc_Addr.c_Label, 1)
                .ARRANGE_TOPS(FPc_Addr_Type.c_Label, FPc_Addr_Type.c)

                .SIZE_HEIGHT_TO_MAX(Address_Panel)
            End With
        End If
    End Sub

    Private Sub SET_LAYOUT_WIDTH()
        If FPc_Addr_ps_type.P_VALUE = "#_SIMPLE_#" Then
            Call SET_LAYOUT_WIDTH_SIMPLE()
        Else
            Call SET_LAYOUT_WIDTH_NORMAL()
        End If
    End Sub

    Public Sub SET_LAYOUT()
        SET_LAYOUT_WIDTH()
    End Sub

    Public Function Addr_Values_CHECK(CUST_ID As Long, Country As String, ZIP As String, District As String, City As String, Addr As String, Addr_ps_type As String, Addr_housenr As String, Addr_building As String, Addr_stairway As String, Addr_floor As String, Addr_door As String, Optional WithDialog As Boolean = True) As Boolean
        Dim OUT As Boolean

        OUT = True

        If CUST_ID <> 0 Then
            If Addr_ps_type <> "#_SIMPLE_#" Then
                If Trim(Addr_ps_type) = "" Then
                    If WithDialog Then
                        If gl_FPApp.DoMyMsgBox(3148, "", "SEQ,YES", "SEQ,NO") = 1 Then
                            '+++Call UserGl.Form_OpenForm("Cegek_adatai", , , Str$(CUST_ID))
                        End If
                    End If
                End If
            End If
        End If

        Addr_Values_CHECK = OUT
    End Function

    Private Sub FP_Form_BeginEdit(sender_FP As FP) Handles FP.Form_BeginEdit
        If P_Spec_Addr_Default_Value = True Then
            If Not (FPc_Addr_Type Is Nothing) Then
                If Not (FPc_Addr_Type.c Is Nothing) Then
                    FPc_Addr_Type.P_VALUE = True
                    FPc_Addr_ps_type.P_VALUE = "#_SIMPLE_#"
                End If
            End If
        End If
    End Sub

    Private Sub FP_Form_Controls_Arrange_Begin(sender_FP As FP) Handles FP.Form_Controls_Arrange_Begin
        SET_LAYOUT_HEIGHT()
    End Sub

    Private Sub FP_Form_Controls_Arrange_End(sender_FP As FP) Handles FP.Form_Controls_Arrange_End
        SET_LAYOUT_WIDTH()
    End Sub

    Private Sub FPc_Addr_Type_TextChanged(sender_FPc As FP_Control, sender As Object, e As EventArgs, ByRef Cancel As Boolean) Handles FPc_Addr_Type.Field_TextChanged
        If FPc_Addr_Type.P_VALUE = False Then
            FPc_Addr_ps_type.P_VALUE = ""
        Else
            If gl_FPApp.DoMyMsgBox(24051, , "SEQ,NEIN", "SEQ,JA") <> 2 Then
                Cancel = True
            Else
                Cancel = (FP.FORM_DIRTY_SET = False)
                If Cancel = False Then
                    FPc_Addr_ps_type.P_VALUE = "#_SIMPLE_#"
                    FPc_District.P_VALUE = ""
                    FPc_Addr_housenr.P_VALUE = ""
                    FPc_Addr_building.P_VALUE = ""
                    FPc_Addr_stairway.P_VALUE = ""
                    FPc_Addr_floor.P_VALUE = ""
                    FPc_Addr_door.P_VALUE = ""
                End If
            End If
        End If

        If Cancel = False Then
            SET_LAYOUT_WIDTH()
        End If
    End Sub

    Private Sub Address_Panel_VisibleChanged(sender As Object, e As EventArgs) Handles Address_Panel.VisibleChanged
        If Not Disposed Then
            SET_LAYOUT_WIDTH()
            SET_LAYOUT_HEIGHT()
        End If
    End Sub
End Class
