namespace CSVTest
{
    class EquipmentStatus
    {
        public string equipmentStatus;
        public string availability;
        public string usage;

        public string[] getStringLineFormat(EquipmentStatus s)
        {
            string[] output = new string[3]{s.equipmentStatus, s.availability, s.usage};

            return output;
        }
    }
}