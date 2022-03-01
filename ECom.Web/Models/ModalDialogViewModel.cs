namespace ECom.Web.Models
{
    public class ModalDialogViewModel
    {
        private string _parentDivId = "";
        private string _childDivId = "";
        private string _title = "";
        private string _style = " ";

        public string ParentDivId
        {
            get
            {
                return _parentDivId;
            }
            set
            {
                _parentDivId = value;
            }
        }

        public string ChildDivId
        {
            get
            {
                return _childDivId;
            }
            set
            {
                _childDivId = value;
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }

        public string Style
        {
            get
            {
                return _style;
            }
            set
            {
                _style = value;
            }
        }
    }
}
