using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.IO;


public partial class Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            // Execute PowerShell script to create folders based on file dates
            ExecutePowerShellScript();

            DirectoryInfo rootInfo = new DirectoryInfo(Server.MapPath("~/Files/"));
            this.PopulateTreeView(rootInfo, null);
        }
    }

    private void PopulateTreeView(DirectoryInfo dirInfo, TreeNode treeNode)
{
    foreach (DirectoryInfo directory in dirInfo.GetDirectories())
    {
        // Check if the directory is empty
        if (directory.GetFiles().Length == 0 && directory.GetDirectories().Length == 0)
            continue; // Skip empty directories

        TreeNode directoryNode = new TreeNode
        {
            Text = directory.Name,
            Value = directory.FullName
        };

        if (treeNode == null)
        {
            // If Root Node, add to TreeView.
            TreeView1.Nodes.Add(directoryNode);
        }
        else
        {
            // If Child Node, add to Parent Node.
            treeNode.ChildNodes.Add(directoryNode);
        }

        // Get all files in the Directory.
        foreach (FileInfo file in directory.GetFiles())
        {
            // Add each file as Child Node.
            TreeNode fileNode = new TreeNode
            {
                Text = file.Name,
                Value = file.FullName,
                Target = "_blank",
                NavigateUrl = (new Uri(Server.MapPath("~/"))).MakeRelativeUri(new Uri(file.FullName)).ToString()
            };
            directoryNode.ChildNodes.Add(fileNode);
        }

        PopulateTreeView(directory, directoryNode);
    }
}


	private void ExecutePowerShellScript()
    {
        string scriptPath = Server.MapPath("~/Scripts/CreateFoldersAndMoveFiles.ps1");

        // Configure the PowerShell process
        ProcessStartInfo processInfo = new ProcessStartInfo();
        processInfo.FileName = "powershell.exe";
        processInfo.Arguments = "-ExecutionPolicy Bypass -File \"" + scriptPath + "\"";
        processInfo.CreateNoWindow = true;
        processInfo.UseShellExecute = false;

        // Start the PowerShell process
        using (Process process = new Process())
        {
            process.StartInfo = processInfo;
            process.Start();
            process.WaitForExit();
        }
    }
}