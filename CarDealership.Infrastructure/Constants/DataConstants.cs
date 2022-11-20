namespace CarDealership.Infrastucture.Constants
{
    /// <summary>
    /// Db Entities constants
    /// </summary>
    public static class DataConstants
    {
        /// <summary>
        /// Dealer constants
        /// </summary>
        public static class Dealer
        {
            /// <summary>
            /// User properties max/min length symbols
            /// </summary>
            public const int UserFirstNamelMaxLength = 20;
            public const int UserLastNamelMaxLength = 20;
        }
        /// <summary>
        /// Car constants
        /// </summary>
        public static class Car
        {
            /// <summary>
            /// Car properties max/min length symbols
            /// </summary>
            public const int ModelMaxLength = 50;
            public const int DescriptionMaxLength = 500;
            public const int ImageUrlMaxLength = 200;
        }

        /// <summary>
        /// CarCategory constants
        /// </summary>
        public static class CarCategory
        {
            /// <summary>
            /// CarCategory properties max/min lenght symbols
            /// </summary>
            public const int NameMaxLength = 50;
        }

        
    }
}
