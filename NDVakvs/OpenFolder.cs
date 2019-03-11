using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NDVakvs
{
    class OpenFolder
    {

        public void openFolderWithSrc()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = @"C:\";
            fbd.ShowNewFolderButton = false;

            if(fbd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    InsertMarker insertMarker = new InsertMarker(fbd);
                    insertMarker.insertMarker();
                    //FindJsFilesInAllFiles findJsFilesInAllFiles = new FindJsFilesInAllFiles(fbd);
                    //findJsFilesInAllFiles.findJsFilesInSrc();
                }
                catch(Exception e)
                {
                    MessageBox.Show("Error: can't open folder.\nOriginal error: "+ e.Message);
                }
            }
        }

        public void openFolderWithBin()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = @"C:\";
            fbd.ShowNewFolderButton = false;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error: can't open folder.\nOriginal error: " + e.Message);
                }
            }
        }

    }
}
