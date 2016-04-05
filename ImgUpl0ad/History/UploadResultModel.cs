using System;
using System.Windows;

using PrincessLuna;

namespace ImgUpl0ad.History
{
    public class UploadResultModel : ObservableObject
    {
        #region fields

        private string _directLink;
        private string _originalname;
        private Point _resolution;
        private long _fileSize;
        private DateTime _uploadedDate;
        private string _deleteHash;

        #endregion

        public string DirectLink
        {
            get { return _directLink; }
            set
            {
                if (value == _directLink)
                    return;

                _directLink = value;
                OnPropertyChanged(nameof(DirectLink));
            }
        }

        public string OriginalName
        {
            get { return _originalname; }
            set
            {
                if (value == _originalname)
                    return;

                _originalname = value;
                OnPropertyChanged(nameof(OriginalName));
            }
        }

        public Point Resolution
        {
            get { return _resolution; }
            set
            {
                if (value == _resolution)
                    return;

                _resolution = value;
                OnPropertyChanged(nameof(Resolution));
            }
        }

        public long FileSize
        {
            get { return _fileSize; }
            set
            {
                if (value == _fileSize)
                    return;

                _fileSize = value;
                OnPropertyChanged(nameof(FileSize));
            }
        }

        public DateTime UploadedDate
        {
            get { return _uploadedDate; }
            set
            {
                if (value == _uploadedDate)
                    return;

                _uploadedDate = value;
                OnPropertyChanged(nameof(UploadedDate));
            }
        }

        public string DeleteHash
        {
            get { return _deleteHash; }
            set
            {
                if (value == _deleteHash)
                    return;

                _deleteHash = value;
                OnPropertyChanged(nameof(_deleteHash));
            }
        }
    }
}
