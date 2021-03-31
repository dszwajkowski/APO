using System.Drawing;

namespace ApoCore
{
    public class ImageModel
    {
        #region Public properties 
        public Bitmap Image
        {
            get => image;
            set
            {
                if (image == value)
                    return;
                image = value;
                //HistogramData.GetHistogramData();
            }
        }
        public string ImagePath { get; set; }
        public HistogramModel HistogramData { get; set; }
        #endregion

        #region Private properties
        private Bitmap image;
        #endregion

        #region Constructors
        public ImageModel(string imagepath)
        {
            this.Image = new(imagepath);
            this.ImagePath = imagepath;
            HistogramData = new HistogramModel(this.Image);
        }

        public ImageModel(Bitmap image)
        {
            this.Image = new(image);
            this.ImagePath = "none";
            HistogramData = new HistogramModel(this.Image);
        }
        #endregion
    }
}
