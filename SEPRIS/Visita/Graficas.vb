<Serializable>
Public Class Graficas
   Private _visitasVF As Integer
   Private _visitasVO As Integer
   Private _visitasCGIV As Integer
   Private _visitasPLD As String
   Private _totalAreaEst As Integer
   Private _estatusVisita As String
   Private _areaVisita As String

   Private _totalVigentes As Integer
   Private _totalConProrroga As Integer
   Private _totalProcEmplaza As Integer
   Private _totalEmplazadas As Integer
   Private _totalProcSan As Integer
   Private _totalSancionadas As Integer
   Private _totalEspAccJurid As Integer
   Private _totalRevoca As Integer
   Private _totalJuicioNul As Integer
   Private _totalCanceladas As Integer
   Private _totalCerradas As Integer

   Private _totalIntegrales As Integer
   Private _totalSeguimientos As Integer
   Private _totalEspeciales As Integer
   Private _totalOrientadas As Integer
   Private _totalOrdinarias As Integer

   Private _totalAzteca As Integer
   Private _totalBanamex As Integer
   Private _totalCoppel As Integer
   Private _totalInbursa As Integer
   Private _totalInvercap As Integer
   Private _totalMetlife As Integer
   Private _totalPension As Integer
   Private _totalPrincipal As Integer
   Private _totalProfuturo As Integer
   Private _totalSura As Integer
   Private _totalBanorte As Integer

   Public Property TotalProfuturo() As Integer
      Get
         Return _totalProfuturo
      End Get
      Set(ByVal value As Integer)
         _totalProfuturo = value
      End Set
   End Property

   Public Property TotalSura() As Integer
      Get
         Return _totalSura
      End Get
      Set(ByVal value As Integer)
         _totalSura = value
      End Set
   End Property

   Public Property TotalBanorte() As Integer
      Get
         Return _totalBanorte
      End Get
      Set(ByVal value As Integer)
         _totalBanorte = value
      End Set
   End Property

   Public Property TotalPension() As Integer
      Get
         Return _totalPension
      End Get
      Set(ByVal value As Integer)
         _totalPension = value
      End Set
   End Property

   Public Property TotalPrincipal() As Integer
      Get
         Return _totalPrincipal
      End Get
      Set(ByVal value As Integer)
         _totalPrincipal = value
      End Set
   End Property

   Public Property TotalInvercap() As Integer
      Get
         Return _totalInvercap
      End Get
      Set(ByVal value As Integer)
         _totalInvercap = value
      End Set
   End Property

   Public Property TotalMetlife() As Integer
      Get
         Return _totalMetlife
      End Get
      Set(ByVal value As Integer)
         _totalMetlife = value
      End Set
   End Property

   Public Property TotalCoppel() As Integer
      Get
         Return _totalCoppel
      End Get
      Set(ByVal value As Integer)
         _totalCoppel = value
      End Set
   End Property

   Public Property TotalInbursa() As Integer
      Get
         Return _totalInbursa
      End Get
      Set(ByVal value As Integer)
         _totalInbursa = value
      End Set
   End Property

   Public Property TotalAzteca() As Integer
      Get
         Return _totalAzteca
      End Get
      Set(ByVal value As Integer)
         _totalAzteca = value
      End Set
   End Property

   Public Property TotalBanamex() As Integer
      Get
         Return _totalBanamex
      End Get
      Set(ByVal value As Integer)
         _totalBanamex = value
      End Set
   End Property

   Public Property TotalOrdinarias() As Integer
      Get
         Return _totalOrdinarias
      End Get
      Set(ByVal value As Integer)
         _totalOrdinarias = value
      End Set
   End Property

   Public Property TotalOrientadas() As Integer
      Get
         Return _totalOrientadas
      End Get
      Set(ByVal value As Integer)
         _totalOrientadas = value
      End Set
   End Property

   Public Property TotalEspeciales() As Integer
      Get
         Return _totalEspeciales
      End Get
      Set(ByVal value As Integer)
         _totalEspeciales = value
      End Set
   End Property

   Public Property TotalSeguimientos() As Integer
      Get
         Return _totalSeguimientos
      End Get
      Set(ByVal value As Integer)
         _totalSeguimientos = value
      End Set
   End Property

   Public Property TotalIntegrales() As Integer
      Get
         Return _totalIntegrales
      End Get
      Set(ByVal value As Integer)
         _totalIntegrales = value
      End Set
   End Property

   Public Property TotalJuicioNul() As Integer
      Get
         Return _totalJuicioNul
      End Get
      Set(ByVal value As Integer)
         _totalJuicioNul = value
      End Set
   End Property

   Public Property TotalCanceladas() As Integer
      Get
         Return _totalCanceladas
      End Get
      Set(ByVal value As Integer)
         _totalCanceladas = value
      End Set
   End Property

   Public Property TotalCerradas() As Integer
      Get
         Return _totalCerradas
      End Get
      Set(ByVal value As Integer)
         _totalCerradas = value
      End Set
   End Property

   Public Property TotalEmplazadas() As Integer
      Get
         Return _totalEmplazadas
      End Get
      Set(ByVal value As Integer)
         _totalEmplazadas = value
      End Set
   End Property

   Public Property TotalProcSan() As Integer
      Get
         Return _totalProcSan
      End Get
      Set(ByVal value As Integer)
         _totalProcSan = value
      End Set
   End Property

   Public Property TotalSancionadas() As Integer
      Get
         Return _totalSancionadas
      End Get
      Set(ByVal value As Integer)
         _totalSancionadas = value
      End Set
   End Property

   Public Property TotalEspAccJurid() As Integer
      Get
         Return _totalEspAccJurid
      End Get
      Set(ByVal value As Integer)
         _totalEspAccJurid = value
      End Set
   End Property

   Public Property TotalVigentes() As Integer
      Get
         Return _totalVigentes
      End Get
      Set(ByVal value As Integer)
         _totalVigentes = value
      End Set
   End Property

   Public Property TotalConProrroga() As Integer
      Get
         Return _totalConProrroga
      End Get
      Set(ByVal value As Integer)
         _totalConProrroga = value
      End Set
   End Property

   Public Property TotalProcEmplaza() As Integer
      Get
         Return _totalProcEmplaza
      End Get
      Set(ByVal value As Integer)
         _totalProcEmplaza = value
      End Set
   End Property

   Public Property TotalRevoca() As Integer
      Get
         Return _totalRevoca
      End Get
      Set(ByVal value As Integer)
         _totalRevoca = value
      End Set
   End Property

   Public Property AreaVisita() As String
      Get
         Return _areaVisita
      End Get
      Set(ByVal value As String)
         _areaVisita = value
      End Set
   End Property

   Public Property EstatusVisita() As String
      Get
         Return _estatusVisita
      End Get
      Set(ByVal value As String)
         _estatusVisita = value
      End Set
   End Property

   Public Property TotalAreaEst() As Integer
      Get
         Return _totalAreaEst
      End Get
      Set(ByVal value As Integer)
         _totalAreaEst = value
      End Set
   End Property

   Public Property VisitasVF() As Integer
      Get
         Return _visitasVF
      End Get
      Set(ByVal value As Integer)
         _visitasVF = value
      End Set
   End Property

   Public Property VisitasVO() As Integer
      Get
         Return _visitasVO
      End Get
      Set(ByVal value As Integer)
         _visitasVO = value
      End Set
   End Property

   Public Property VisitasCGIV() As Integer
      Get
         Return _visitasCGIV
      End Get
      Set(ByVal value As Integer)
         _visitasCGIV = value
      End Set
   End Property

   Public Property VisitasPLD() As Integer
      Get
         Return _visitasPLD
      End Get
      Set(ByVal value As Integer)
         _visitasPLD = value
      End Set
   End Property

End Class
