Public Delta As Double

Public Район As Double
Public Адрес As Double
Public ИндексПоРп As Double
Public ДопДанные As Double

Public ЭС As Double
Public ТС As Double
Public ГС As Double
Public ХВС As Double
Public ГВС As Double
Public ВО As Double
Public Фунд As Double
Public АППЗ As Double
Public Подвал As Double
Public Лифты As Double
Public Крыша As Double
Public Фасад As Double
Public Аварийка As Double
Public ПД As Double

Public Sub ВыполнитьПереносДанных()

СтолбцыНовогоПлана
trans 14, 2820, "970", "R970"

СтолбцыСтарогоПлана
trans 14, 3240, "814", "R814"

End Sub

Public Function СтолбцыНовогоПлана()
    
    Район = 6
    Адрес = 8
    ИндексПоРп = 2
    ДопДанные = 4 'данные которые проще достать в пред обработке
    
    ЭС = 18
    ТС = 19
    ГС = 20
    ХВС = 21
    ГВС = 22
    ВО = 23
    Фунд = 24
    АППЗ = 25
    Подвал = 26
    Лифты = 27
    Крыша = 28
    Фасад = 29
    Аварийка = 30
    ПД = 31
End Function
Public Function СтолбцыСтарогоПлана()
   
    
    Район = 6
    Адрес = 8
    ИндексПоРп = 2
    ДопДанные = 4 'данные которые проще достать в пред обработке
    
    ЭС = 19
    ТС = 21
    ГС = 23
    ХВС = 25
    ГВС = 27
    ВО = 29
    Фунд = 31
    АППЗ = 33
    Подвал = 35
    Лифты = 37
    Крыша = 39
    Фасад = 41
    Аварийка = 43
    ПД = 44
End Function


Function trans(ByVal StartRow As Double, ByVal EndRow As Double, ByVal DataListName As String, ByVal ResultListName As String)
    Dim ResultRow As Double
    ResultRow = 2

SetColumnHeaders ResultListName 'Заголовки

Application.ScreenUpdating = False
Application.Calculation = xlCalculationManual
    Dim i As Double

    For i = StartRow To EndRow
        If Worksheets(DataListName).Cells(i, Адрес) <> "" And Worksheets(DataListName).Cells(i, Район) <> "" Then
        
            SetData ResultListName, ResultRow + 0, "ЭС", DataListName, i, ЭС
            SetData ResultListName, ResultRow + 1, "ТС", DataListName, i, ТС
            SetData ResultListName, ResultRow + 2, "ГС", DataListName, i, ГС
            SetData ResultListName, ResultRow + 3, "ХВС", DataListName, i, ХВС
            SetData ResultListName, ResultRow + 4, "ГВС", DataListName, i, ГВС
            SetData ResultListName, ResultRow + 5, "ВО", DataListName, i, ВО
            SetData ResultListName, ResultRow + 6, "Фунд", DataListName, i, Фунд
            SetData ResultListName, ResultRow + 7, "АППЗ", DataListName, i, АППЗ
            SetData ResultListName, ResultRow + 8, "Подвал", DataListName, i, Подвал
            SetData ResultListName, ResultRow + 9, "Лифты", DataListName, i, Лифты
            SetData ResultListName, ResultRow + 10, "Крыша", DataListName, i, Крыша
            SetData ResultListName, ResultRow + 11, "Фасад", DataListName, i, Фасад
            SetData ResultListName, ResultRow + 12, "Аварийка", DataListName, i, Аварийка
            SetData ResultListName, ResultRow + 13, "ПД", DataListName, i, ПД
            
            ResultRow = ResultRow + 14
        Else: End If
    Next i
    
Application.ScreenUpdating = True
Application.Calculation = xlCalculationAutomatic

End Function



Function SetData( _
                        Имя_листа_с_отчетом As String, _
                        Номер_строки_отчета As Double, _
                        Наименование_работ As String, _
                        Имя_листа_с_данными As String, _
                        Номер_строки_с_данными As Double, _
                        Номер_столбца_с_данными As Double)

        Worksheets(Имя_листа_с_отчетом).Cells(Номер_строки_отчета, 1) = Worksheets(Имя_листа_с_данными).Cells(Номер_строки_с_данными, Район)  'район
        Worksheets(Имя_листа_с_отчетом).Cells(Номер_строки_отчета, 2) = Worksheets(Имя_листа_с_данными).Cells(Номер_строки_с_данными, Адрес)  'район
        Worksheets(Имя_листа_с_отчетом).Cells(Номер_строки_отчета, 3) = Worksheets(Имя_листа_с_данными).Cells(Номер_строки_с_данными, ИндексПоРп)  'ИндексПоРп
        Worksheets(Имя_листа_с_отчетом).Cells(Номер_строки_отчета, 4) = Worksheets(Имя_листа_с_данными).Cells(Номер_строки_с_данными, ДопДанные)  'дополнительные данные
        
        Worksheets(Имя_листа_с_отчетом).Cells(Номер_строки_отчета, 5) = Наименование_работ  'Наименование работ
        Worksheets(Имя_листа_с_отчетом).Cells(Номер_строки_отчета, 6) = Worksheets(Имя_листа_с_данными).Cells(Номер_строки_с_данными, Номер_столбца_с_данными) 'данные по стоимости

End Function

Function SetColumnHeaders(Имя_листа_с_отчетом As String)
        Worksheets(Имя_листа_с_отчетом).Cells(1, 1) = "Район"
        Worksheets(Имя_листа_с_отчетом).Cells(1, 2) = "Адрес"
        Worksheets(Имя_листа_с_отчетом).Cells(1, 3) = "Позиция по РП"
        Worksheets(Имя_листа_с_отчетом).Cells(1, 4) = "Дополнительные данные"
        Worksheets(Имя_листа_с_отчетом).Cells(1, 5) = "Вид работ"
        Worksheets(Имя_листа_с_отчетом).Cells(1, 6) = "Стоимость"
End Function
