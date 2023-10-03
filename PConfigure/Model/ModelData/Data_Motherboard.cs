namespace PConfigure.Model
{
	internal class Data_Motherboard
	{
		public int ID { get; set; }
		public string? Name { get; set; }
		public string? TypeATX { get; set; }
		public string? Socket { get; set; }
		public string? Chipset { get; set; }
		public int TypeDDR { get; set; }
		public int TypeGDDR { get; set; }
		public int CountSATA3 { get; set; }
		public int CountM2 { get; set; }
		public double Price { get; set; }
	}
}
