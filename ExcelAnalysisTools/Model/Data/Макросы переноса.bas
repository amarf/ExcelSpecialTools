Public Delta As Double

Public ����� As Double
Public ����� As Double
Public ���������� As Double
Public ��������� As Double

Public �� As Double
Public �� As Double
Public �� As Double
Public ��� As Double
Public ��� As Double
Public �� As Double
Public ���� As Double
Public ���� As Double
Public ������ As Double
Public ����� As Double
Public ����� As Double
Public ����� As Double
Public �������� As Double
Public �� As Double

Public Sub ����������������������()

������������������
trans 14, 2820, "970", "R970"

�������������������
trans 14, 3240, "814", "R814"

End Sub

Public Function ������������������()
    
    ����� = 6
    ����� = 8
    ���������� = 2
    ��������� = 4 '������ ������� ����� ������� � ���� ���������
    
    �� = 18
    �� = 19
    �� = 20
    ��� = 21
    ��� = 22
    �� = 23
    ���� = 24
    ���� = 25
    ������ = 26
    ����� = 27
    ����� = 28
    ����� = 29
    �������� = 30
    �� = 31
End Function
Public Function �������������������()
   
    
    ����� = 6
    ����� = 8
    ���������� = 2
    ��������� = 4 '������ ������� ����� ������� � ���� ���������
    
    �� = 19
    �� = 21
    �� = 23
    ��� = 25
    ��� = 27
    �� = 29
    ���� = 31
    ���� = 33
    ������ = 35
    ����� = 37
    ����� = 39
    ����� = 41
    �������� = 43
    �� = 44
End Function


Function trans(ByVal StartRow As Double, ByVal EndRow As Double, ByVal DataListName As String, ByVal ResultListName As String)
    Dim ResultRow As Double
    ResultRow = 2

SetColumnHeaders ResultListName '���������

Application.ScreenUpdating = False
Application.Calculation = xlCalculationManual
    Dim i As Double

    For i = StartRow To EndRow
        If Worksheets(DataListName).Cells(i, �����) <> "" And Worksheets(DataListName).Cells(i, �����) <> "" Then
        
            SetData ResultListName, ResultRow + 0, "��", DataListName, i, ��
            SetData ResultListName, ResultRow + 1, "��", DataListName, i, ��
            SetData ResultListName, ResultRow + 2, "��", DataListName, i, ��
            SetData ResultListName, ResultRow + 3, "���", DataListName, i, ���
            SetData ResultListName, ResultRow + 4, "���", DataListName, i, ���
            SetData ResultListName, ResultRow + 5, "��", DataListName, i, ��
            SetData ResultListName, ResultRow + 6, "����", DataListName, i, ����
            SetData ResultListName, ResultRow + 7, "����", DataListName, i, ����
            SetData ResultListName, ResultRow + 8, "������", DataListName, i, ������
            SetData ResultListName, ResultRow + 9, "�����", DataListName, i, �����
            SetData ResultListName, ResultRow + 10, "�����", DataListName, i, �����
            SetData ResultListName, ResultRow + 11, "�����", DataListName, i, �����
            SetData ResultListName, ResultRow + 12, "��������", DataListName, i, ��������
            SetData ResultListName, ResultRow + 13, "��", DataListName, i, ��
            
            ResultRow = ResultRow + 14
        Else: End If
    Next i
    
Application.ScreenUpdating = True
Application.Calculation = xlCalculationAutomatic

End Function



Function SetData( _
                        ���_�����_�_������� As String, _
                        �����_������_������ As Double, _
                        ������������_����� As String, _
                        ���_�����_�_������� As String, _
                        �����_������_�_������� As Double, _
                        �����_�������_�_������� As Double)

        Worksheets(���_�����_�_�������).Cells(�����_������_������, 1) = Worksheets(���_�����_�_�������).Cells(�����_������_�_�������, �����)  '�����
        Worksheets(���_�����_�_�������).Cells(�����_������_������, 2) = Worksheets(���_�����_�_�������).Cells(�����_������_�_�������, �����)  '�����
        Worksheets(���_�����_�_�������).Cells(�����_������_������, 3) = Worksheets(���_�����_�_�������).Cells(�����_������_�_�������, ����������)  '����������
        Worksheets(���_�����_�_�������).Cells(�����_������_������, 4) = Worksheets(���_�����_�_�������).Cells(�����_������_�_�������, ���������)  '�������������� ������
        
        Worksheets(���_�����_�_�������).Cells(�����_������_������, 5) = ������������_�����  '������������ �����
        Worksheets(���_�����_�_�������).Cells(�����_������_������, 6) = Worksheets(���_�����_�_�������).Cells(�����_������_�_�������, �����_�������_�_�������) '������ �� ���������

End Function

Function SetColumnHeaders(���_�����_�_������� As String)
        Worksheets(���_�����_�_�������).Cells(1, 1) = "�����"
        Worksheets(���_�����_�_�������).Cells(1, 2) = "�����"
        Worksheets(���_�����_�_�������).Cells(1, 3) = "������� �� ��"
        Worksheets(���_�����_�_�������).Cells(1, 4) = "�������������� ������"
        Worksheets(���_�����_�_�������).Cells(1, 5) = "��� �����"
        Worksheets(���_�����_�_�������).Cells(1, 6) = "���������"
End Function
