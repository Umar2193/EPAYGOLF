using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
	[Serializable]
	public class ApplicationExceptions : ApplicationException
	{
		public static event EventHandler<ExceptionForLogArgs> evtUserMessageRecieved;

		public static string CONTACTSYSTEMADMINISTRATOR = Environment.NewLine + @" Contact your system administrator for details.";
		public static string SERVICEERROR = "Application is unable to connect to server." + CONTACTSYSTEMADMINISTRATOR;
		public static string DATABASEPERMISSIONERROR = "You do not have sufficient permissions to access application database." + CONTACTSYSTEMADMINISTRATOR;
		public static string DATABASEDOWNERROR = "Database connection is down, possible reason may be network failure." + CONTACTSYSTEMADMINISTRATOR;
		public static string OPERATIONTIMEDOUT = String.Format(@"The operation has timed out.{0}Possible reasons may be{0}.Network failure {0}.Web Services unavailability {0}.Database unavailability.{1}", Environment.NewLine, CONTACTSYSTEMADMINISTRATOR);
		public static string NETWORKERROR = "It appears that your system is not on network." + CONTACTSYSTEMADMINISTRATOR;
		public static string DATABASEOPEN = "Unable to open database. Check your database connection settings." + CONTACTSYSTEMADMINISTRATOR;
		public static string SERVICESCONNECTION = "Application is unable to connect with Web Services." + CONTACTSYSTEMADMINISTRATOR;
		public static string MAPPOINTERROR = String.Format("MapPoint Europe cannot run because it is not registered on your system, or it cannot be found.{0}Install MapPoint Europe and try again.{1}", Environment.NewLine, CONTACTSYSTEMADMINISTRATOR);
		public static string LOGINFAILED = "  Application is unable to log you in with the given credentials. Try again later.";
		public static string USERROLE = "You don't have permissions to view this page.";
		public static string TASKSTATUS = "Application is unable to change status. Please contact system administration.";
		public static string PRINTERUNAVAILABLE = "Printer is not accessible.";
		public static string MACHINREGISTRATION = "Machine is not registered." + CONTACTSYSTEMADMINISTRATOR;
		public static string INVALIDLOGINPASSWORD = "Your login credentials are invalid/inactive." + CONTACTSYSTEMADMINISTRATOR;
		public static string INACTIVEMACHINE = "Machine is not active." + CONTACTSYSTEMADMINISTRATOR;
		public static string GENERALERROR = "An error has occurred while processing your request." + CONTACTSYSTEMADMINISTRATOR;

		public static string INCONSISTENTDATAERROR = "The data is inconsistent, all screens will be refreshed/closed now.";
		public static string _BaselogPath = Directory.GetCurrentDirectory() + "\\wwwroot\\Logs";

        //public static Boolean ShowErrors = false;

        /// <summary>
        /// Exception Event
        /// </summary>
        //public static event EventHandler<ExceptionArgs> evtExceptionOccoured;

        #region Public Methods

        /// <summary>
        /// Method for Handling Exception
        /// </summary>
        /// <param name="pobjExp"></param>
        public static void HandleAppExc(Exception pobjExp)
		{
			HandleAppExc(null, new ExceptionArgs(pobjExp, null));
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="pobjExp"></param>
		public static void HandleAppExc(object pobjSender, ExceptionArgs expArgs)
		{
			//if (BusinessRule.Utilities.ShowException)
			//{
			if (evtUserMessageRecieved != null)
			{
				evtUserMessageRecieved(pobjSender, new ExceptionForLogArgs(GetUserFriendlyMessage(expArgs.ExceptionObj), expArgs.ExceptionObj.Message, expArgs.ExceptionObj));
			}
			//}
		}

		public static string GetUserFriendlyMessage(Exception expArgs)
		{
			return GetMessageToShow(expArgs.Message);
		}

		public static string GetMessageToShow(string errorMessage)
		{
			if (errorMessage.Contains("SOAP"))
			{
				return SERVICEERROR;
			}
			else if (errorMessage.Contains(USERROLE))
			{
				return USERROLE;
			}
			else if (errorMessage.ToUpper().Contains("SQL SERVER DOES NOT EXIST OR ACCESS DENIED") ||
					errorMessage.ToUpper().Contains("GENERAL NETWORK ERROR") ||
					errorMessage.Contains("Unable to connect to the remote server") ||
					errorMessage.Contains("The remote name could not be resolved") ||
					errorMessage.Contains("The request was aborted") ||
					errorMessage.Contains("The server was not found or was not accessible") ||
					errorMessage.Contains("Client found response content") ||
					errorMessage.Contains("no connection could be made because the target machine actively refused it") ||
					errorMessage.Contains("502") ||
					errorMessage.Contains("400") ||
					errorMessage.Contains("404") ||
					errorMessage.ToLower().Contains("making the result invalid") ||
					errorMessage.Contains("endpoint") ||
					errorMessage.Contains("endpoint") ||
					errorMessage.ToLower().Contains("there was no endpoint listening")
				//  errorMessage.Contains("An exception occurred during the operation")
				)
			{
				return SERVICEERROR;
			}
			else if (errorMessage.ToUpper().Contains("PERMISSION DENIED"))
			{
				return DATABASEPERMISSIONERROR;
			}
			else if (errorMessage.Contains("An error has occurred while establishing a connection to the server"))
			{
				return DATABASEDOWNERROR;
			}
			else if (errorMessage.Contains("The operation has timed out")
				|| errorMessage.Contains("Timeout expired.")
				|| errorMessage.Contains("The time allotted to this operation may have been a portion of a longer timeout")
				|| errorMessage.Contains("has exceeded the allotted timeout")
				)
			{
				return SERVICEERROR;
				// Commented By Cherin - Return Service Error even for Time out
				//return OPERATIONTIMEDOUT;
			}
			else if (errorMessage.Contains("TCP")
			   || errorMessage.Contains("A transport-level error has occurred when sending the request to the server")
			   || errorMessage.Contains("The remote name could not be resolved")
				|| errorMessage.Contains("A network-related or instance-specific error occurred"))
			{
				return NETWORKERROR;
			}
			else if (errorMessage.Contains("Cannot open database"))
			{
				return DATABASEOPEN;
			}
			else if (errorMessage.Contains("The underlying connection was closed") || errorMessage.Contains("There is an error in XML document"))
			{
				return SERVICESCONNECTION;
			}
			else if (errorMessage.Contains("MapPoint Europe can't run because it is not registered on your system, or it can't be found. Install MapPoint Europe and try again"))
			{
				return MAPPOINTERROR;
			}
			else if (errorMessage.Contains("Invalid user name or password"))
			{
				return LOGINFAILED;
			}
			else if (errorMessage.Contains("The RPC server is unavailable"))
			{
				return PRINTERUNAVAILABLE;
			}
			else if (errorMessage.Contains("Code-50000"))
			{
				return INCONSISTENTDATAERROR;
			}
			else
				return String.Empty;
		}

		public static void SaveLog(Exception exc)
		{
			try
			{
				if (Directory.Exists(_BaselogPath + @"\Exception Log") == false)
					Directory.CreateDirectory(_BaselogPath + @"\Exception Log");

				FileStream objFS = null;
				string strFilePath = String.Format(@"{0}\Exception Log\{1:yyyy-MM-dd }Exception.txt", _BaselogPath, System.DateTime.Now);
				if (!File.Exists(strFilePath))
				{
					objFS = new FileStream(strFilePath, FileMode.Create);
				}
				else
					objFS = new FileStream(strFilePath, FileMode.Append);

				using (StreamWriter Sr = new StreamWriter(objFS))
				{
					Sr.WriteLine(String.Format("Exception Time: {0}" + Environment.NewLine + "TargetSite: {1}" + Environment.NewLine + "Exception Message: {2}" + Environment.NewLine + "Inner Exception: {3} " + Environment.NewLine + "StackTrace: {4}", System.DateTime.Now.ToLongTimeString(), exc.TargetSite, exc.Message, exc.InnerException, exc.ToString()));
					if (!String.IsNullOrEmpty(exc.StackTrace))
					{
						//Sr.WriteLine("---- More Detials ----");
						//// Sr.WriteLine(exc.StackTrace);
						////Get a StackTrace object for the exception
						//StackTrace st = new StackTrace(exc, true);

						////Get the first stack frame
						//StackFrame frame = st.GetFrame(0);

						////Get the file name
						//string fileName = frame.GetFileName();

						//Sr.WriteLine("File Name : "+ fileName);
						////Get the method name
						//string methodName = frame.GetMethod().Name;

						//Sr.WriteLine("Method Name : " + methodName);
						////Get the line number from the stack frame
						//int line = frame.GetFileLineNumber();

						//Sr.WriteLine("Line Number : " + line);

						////Get the column number
						//int col = frame.GetFileColumnNumber();
						//Sr.WriteLine("Column Number : " + col);

					}
					Sr.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
				}
			}
			catch (Exception exp)
			{
				// System.Diagnostics.EventLog.WriteEntry("PMS NZ", "Error saving logs : " + exp.Message);
			}
		}
		public static void SaveLogSessionError(Exception exc)
		{
			try
			{
				if (Directory.Exists(_BaselogPath + @"\Exception Log") == false)
					Directory.CreateDirectory(_BaselogPath + @"\Exception Log");

				FileStream objFS = null;
				string strFilePath = String.Format(@"{0}\Exception Log\{1:yyyy-MM-dd }SessionErrorException.txt", _BaselogPath, System.DateTime.Now);
				if (!File.Exists(strFilePath))
				{
					objFS = new FileStream(strFilePath, FileMode.Create);
				}
				else
					objFS = new FileStream(strFilePath, FileMode.Append);

				using (StreamWriter Sr = new StreamWriter(objFS))
				{
					Sr.WriteLine(String.Format("Exception Time: {0}" + Environment.NewLine + "TargetSite: {1}" + Environment.NewLine + "Exception Message: {2}" + Environment.NewLine + "Inner Exception: {3} " + Environment.NewLine + "StackTrace: {4}", System.DateTime.Now.ToLongTimeString(), exc.TargetSite, exc.Message, exc.InnerException, exc.ToString()));
					if (!String.IsNullOrEmpty(exc.StackTrace))
					{
						//Sr.WriteLine("---- More Detials ----");
						//// Sr.WriteLine(exc.StackTrace);
						////Get a StackTrace object for the exception
						//StackTrace st = new StackTrace(exc, true);

						////Get the first stack frame
						//StackFrame frame = st.GetFrame(0);

						////Get the file name
						//string fileName = frame.GetFileName();

						//Sr.WriteLine("File Name : "+ fileName);
						////Get the method name
						//string methodName = frame.GetMethod().Name;

						//Sr.WriteLine("Method Name : " + methodName);
						////Get the line number from the stack frame
						//int line = frame.GetFileLineNumber();

						//Sr.WriteLine("Line Number : " + line);

						////Get the column number
						//int col = frame.GetFileColumnNumber();
						//Sr.WriteLine("Column Number : " + col);

					}
					Sr.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
				}
			}
			catch (Exception exp)
			{
				// System.Diagnostics.EventLog.WriteEntry("PMS NZ", "Error saving logs : " + exp.Message);
			}
		}
		public static void SaveLogFilterError(Exception exc, string filterType)
		{
			try
			{
				if (Directory.Exists(_BaselogPath + @"\Exception Log") == false)
					Directory.CreateDirectory(_BaselogPath + @"\Exception Log");

				FileStream objFS = null;
				string strFilePath = String.Format(@"{0}\Exception Log\{1:yyyy-MM-dd }FilterErrorException_{2}.txt", _BaselogPath, System.DateTime.Now, filterType);
				if (!File.Exists(strFilePath))
				{
					objFS = new FileStream(strFilePath, FileMode.Create);
				}
				else
					objFS = new FileStream(strFilePath, FileMode.Append);

				using (StreamWriter Sr = new StreamWriter(objFS))
				{
					Sr.WriteLine(String.Format("Exception Time: {0}" + Environment.NewLine + "TargetSite: {1}" + Environment.NewLine + "Exception Message: {2}" + Environment.NewLine + "Inner Exception: {3} " + Environment.NewLine + "StackTrace: {4}", System.DateTime.Now.ToLongTimeString(), exc.TargetSite, exc.Message, exc.InnerException, exc.ToString()));
					if (!String.IsNullOrEmpty(exc.StackTrace))
					{
						//Sr.WriteLine("---- More Detials ----");
						//// Sr.WriteLine(exc.StackTrace);
						////Get a StackTrace object for the exception
						//StackTrace st = new StackTrace(exc, true);

						////Get the first stack frame
						//StackFrame frame = st.GetFrame(0);

						////Get the file name
						//string fileName = frame.GetFileName();

						//Sr.WriteLine("File Name : "+ fileName);
						////Get the method name
						//string methodName = frame.GetMethod().Name;

						//Sr.WriteLine("Method Name : " + methodName);
						////Get the line number from the stack frame
						//int line = frame.GetFileLineNumber();

						//Sr.WriteLine("Line Number : " + line);

						////Get the column number
						//int col = frame.GetFileColumnNumber();
						//Sr.WriteLine("Column Number : " + col);

					}
					Sr.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
				}
			}
			catch (Exception exp)
			{
				// System.Diagnostics.EventLog.WriteEntry("PMS NZ", "Error saving logs : " + exp.Message);
			}
		}
		public static void SaveLogErrorGeneral(Exception exc, string FileName)
		{
			try
			{
				if (Directory.Exists(_BaselogPath + @"\Exception Log") == false)
					Directory.CreateDirectory(_BaselogPath + @"\Exception Log");

				FileStream objFS = null;
				string strFilePath = String.Format(@"{0}\Exception Log\{1:yyyy-MM-dd }{2}_ErrorException.txt", _BaselogPath, System.DateTime.Now, FileName);
				if (!File.Exists(strFilePath))
				{
					objFS = new FileStream(strFilePath, FileMode.Create);
				}
				else
					objFS = new FileStream(strFilePath, FileMode.Append);

				using (StreamWriter Sr = new StreamWriter(objFS))
				{
					Sr.WriteLine(String.Format("Exception Time: {0}" + Environment.NewLine + "TargetSite: {1}" + Environment.NewLine + "Exception Message: {2}" + Environment.NewLine + "Inner Exception: {3} " + Environment.NewLine + "StackTrace: {4}", System.DateTime.Now.ToLongTimeString(), exc.TargetSite, exc.Message, exc.InnerException, exc.ToString()));
					if (!String.IsNullOrEmpty(exc.StackTrace))
					{
						//Sr.WriteLine("---- More Detials ----");
						//// Sr.WriteLine(exc.StackTrace);
						////Get a StackTrace object for the exception
						//StackTrace st = new StackTrace(exc, true);

						////Get the first stack frame
						//StackFrame frame = st.GetFrame(0);

						////Get the file name
						//string fileName = frame.GetFileName();

						//Sr.WriteLine("File Name : "+ fileName);
						////Get the method name
						//string methodName = frame.GetMethod().Name;

						//Sr.WriteLine("Method Name : " + methodName);
						////Get the line number from the stack frame
						//int line = frame.GetFileLineNumber();

						//Sr.WriteLine("Line Number : " + line);

						////Get the column number
						//int col = frame.GetFileColumnNumber();
						//Sr.WriteLine("Column Number : " + col);

					}
					Sr.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
				}
			}
			catch (Exception exp)
			{
				// System.Diagnostics.EventLog.WriteEntry("PMS NZ", "Error saving logs : " + exp.Message);
			}
		}


		public static void SaveLog(string exc)
		{
			try
			{
				if (Directory.Exists(_BaselogPath + @"\Exception Log") == false)
					Directory.CreateDirectory(_BaselogPath + @"\Exception Log");

				FileStream objFS = null;
				string strFilePath = String.Format(@"{0}\Exception Log\{1:yyyy-MM-dd }Exception.txt", _BaselogPath, System.DateTime.Now);
				if (!File.Exists(strFilePath))
				{
					objFS = new FileStream(strFilePath, FileMode.Create);
				}
				else
					objFS = new FileStream(strFilePath, FileMode.Append);

				using (StreamWriter Sr = new StreamWriter(objFS))
				{
					Sr.WriteLine(String.Format("{0}---{1}", System.DateTime.Now.ToShortTimeString(), exc));
				}
			}
			catch
			{
			}
		}

		public static void SaveLogOnSpecificFolder(string FolderName, string exc)
		{
			try
			{
				if (Directory.Exists(_BaselogPath + @"\" + FolderName) == false)
					Directory.CreateDirectory(_BaselogPath + @"\" + FolderName);

				FileStream objFS = null;
				string strFilePath = String.Format(@"{0}\" + FolderName + @"\{1:yyyy-MM-dd }Log.txt", _BaselogPath, System.DateTime.Now);
				if (!File.Exists(strFilePath))
				{
					objFS = new FileStream(strFilePath, FileMode.Create);
				}
				else
					objFS = new FileStream(strFilePath, FileMode.Append);

				using (StreamWriter Sr = new StreamWriter(objFS))
				{
					Sr.WriteLine(String.Format("{0}---{1}", System.DateTime.Now.ToShortTimeString(), exc));
				}
			}
			catch
			{
			}
		}

		public static void SaveLogOnSpecificFolderByUserLoggingID(string FolderName, string exc, int UserID, Int64 UserLoggingID, bool IsProfileLogEnable)
		{
			try
			{
				if (IsProfileLogEnable == true)
				{
					if (Directory.Exists(_BaselogPath + @"\" + FolderName) == false)
						Directory.CreateDirectory(_BaselogPath + @"\" + FolderName);

					FileStream objFS = null;
					string strFileName = string.Concat(UserID, "_", UserLoggingID);
					string strFilePath = String.Format(@"{0}\" + FolderName + @"\{1:yyyy-MM-dd }Log_{2}.txt", _BaselogPath, System.DateTime.Now, strFileName);
					if (File.Exists(strFilePath))
					{
						UpdateActivityLog(strFilePath, 60);
					}
					if (!File.Exists(strFilePath))
					{
						objFS = new FileStream(strFilePath, FileMode.Create);
					}
					else
						objFS = new FileStream(strFilePath, FileMode.Append);

					using (StreamWriter Sr = new StreamWriter(objFS))
					{
						Sr.WriteLine(String.Format("{0}---{1}", System.DateTime.Now, exc));
					}
				}
			}
			catch
			{
			}
		}

		public static void SaveActivityLog(string exc)
		{
			try
			{
				
					if (Directory.Exists(_BaselogPath + @"\Activity Log") == false)
						Directory.CreateDirectory(_BaselogPath + @"\Activity Log");

					FileStream objFS = null;
					string strFilePath = String.Format(@"{0}\Activity Log\{1:yyyy-MM-dd }Activity.txt", _BaselogPath, System.DateTime.Now);
					if (File.Exists(strFilePath))
					{
						UpdateActivityLog(strFilePath, 120);
					}
					if (!File.Exists(strFilePath))
					{
						objFS = new FileStream(strFilePath, FileMode.Create);

					}
					else
						objFS = new FileStream(strFilePath, FileMode.Append);


					// UpdateActivityLog(objFS);


					using (StreamWriter Sr = new StreamWriter(objFS))
					{
						Sr.WriteLine(String.Format("{0:HH:mm:ss}---{1}", System.DateTime.Now, exc));
					}
				
			}
			catch
			{
			}
		}


		public static void UpdateActivityLog(string path, int maxDuration = 60)
		{
			DateTime fileCreationTime = File.GetCreationTime(path);
			string filename = Path.GetFileNameWithoutExtension(path);

			DateTime currentTime = DateTime.Now;

			TimeSpan duration = currentTime - fileCreationTime;
			double min = duration.TotalMinutes;
			// //TimeSpan duration = DateTime.Parse(fileCreationTime.ToString()).Subtract(DateTime.Parse(currentTime.ToString()));
			// //int result = fileCreationTime - currentTime;
			//double hoursDifference = duration.TotalHours;
			if (min > maxDuration)
			{

				string oldFilePath = path;

				// Specify the new file name
				string newFileName = currentTime.ToString("HHmmss");

				// Combine the directory path and the new file name to create the new file path
				string directoryPath = Path.GetDirectoryName(oldFilePath);
				string newFilePath = directoryPath + "\\" + filename + "_" + newFileName + ".txt";

				try
				{
					// Check if the old file exists before renaming it
					if (File.Exists(oldFilePath))
					{
						// Rename the file
						if (!File.Exists(newFilePath))
						{
							File.Move(oldFilePath, newFilePath + ".txt");
						}

						//Console.WriteLine($"File renamed successfully to {newFileName}");
					}
					else
					{
						//Console.WriteLine("The old file does not exist.");
					}
				}
				catch (Exception ex)
				{
					//{
					//    Console.WriteLine($"Error renaming the file: {ex.Message}");
				}



				// }


			}
		}


		public static void SaveAppError(Exception exc)
		{
			try
			{
				if (Directory.Exists(_BaselogPath + @"\Exception Log") == false)
					Directory.CreateDirectory(_BaselogPath + @"\Exception Log");

				FileStream objFS = null;
				string strFilePath = String.Format(@"{0}\Exception Log\{1:yyyy-MM-dd }AppError.txt", _BaselogPath, System.DateTime.Now);
				if (!File.Exists(strFilePath))
				{
					objFS = new FileStream(strFilePath, FileMode.Create);
				}
				else
					objFS = new FileStream(strFilePath, FileMode.Append);

				using (StreamWriter Sr = new StreamWriter(objFS))
				{
					Sr.WriteLine(String.Format("{0}---{1}---{2}---{3}", System.DateTime.Now.ToLongTimeString(), exc.TargetSite, exc.Message, exc.ToString()));
					if (!String.IsNullOrEmpty(exc.StackTrace))
					{
						Sr.WriteLine("----StackTrace----");
						Sr.WriteLine(exc.StackTrace);
					}
				}
			}
			catch
			{
			}
		}
		public static void SaveConnectivityLog(Exception exc)
		{
			try
			{
				if (Directory.Exists(_BaselogPath + @"\Connectivity Log") == false)
					Directory.CreateDirectory(_BaselogPath + @"\Connectivity Log");

				FileStream objFS = null;
				string strFilePath = String.Format(@"{0}\Connectivity Log\{1:yyyy-MM-dd }Connectivity.log", _BaselogPath, System.DateTime.Now);
				if (!File.Exists(strFilePath))
				{
					objFS = new FileStream(strFilePath, FileMode.Create);
				}
				else
					objFS = new FileStream(strFilePath, FileMode.Append);

				using (StreamWriter Sr = new StreamWriter(objFS))
				{
					Sr.WriteLine(String.Format("{0}---{1}---{2}---{3}", System.DateTime.Now.ToLongTimeString(), exc.TargetSite, exc.Message, exc.ToString()));
					if (!String.IsNullOrEmpty(exc.StackTrace))
					{
						Sr.WriteLine("----StackTrace----");
						Sr.WriteLine(exc.StackTrace);
					}
				}
			}
			catch (Exception exp)
			{
				//System.Diagnostics.EventLog.WriteEntry("PMS NZ", "Error saving logs : " + exp.Message);
			}
		}

		public static void SaveLogForTesting(string sMessage, Exception exc)
		{
			try
			{
				if (Directory.Exists(_BaselogPath + @"\Testing Log") == false)
					Directory.CreateDirectory(_BaselogPath + @"\Testing Log");

				FileStream objFS = null;
				string strFilePath = String.Format(@"{0}\Testing Log\{1:yyyy-MM-dd }Testing.log", _BaselogPath, System.DateTime.Now);
				if (!File.Exists(strFilePath))
				{
					objFS = new FileStream(strFilePath, FileMode.Create);
				}
				else
					objFS = new FileStream(strFilePath, FileMode.Append);

				using (StreamWriter Sr = new StreamWriter(objFS))
				{
					Sr.WriteLine(String.Format("{0}---{1}---{2}---{3}", System.DateTime.Now.ToLongTimeString(), exc.TargetSite, exc.Message, exc.ToString(), "Custom Message - > ", sMessage));
					if (!String.IsNullOrEmpty(exc.StackTrace))
					{
						Sr.WriteLine("----StackTrace----");
						Sr.WriteLine(exc.StackTrace);
					}
				}
			}
			catch (Exception exp)
			{
				// System.Diagnostics.EventLog.WriteEntry("PMS NZ", "Error saving logs : " + exp.Message);
			}
		}
		public static void SaveAccessRightsLog(string text, int PracticeID)
		{
			try
			{
				if (Directory.Exists(_BaselogPath + @"\Activity Log\Access Rights\" + PracticeID.ToString()) == false)
					Directory.CreateDirectory(_BaselogPath + @"\Activity Log\Access Rights\" + PracticeID.ToString());

				FileStream objFS = null;
				string strFilePath = String.Format(@"{0}\Activity Log\Access Rights\\" + PracticeID.ToString() + "\\{1:yyyy-MM-dd}.txt", _BaselogPath, System.DateTime.Now);
				if (!File.Exists(strFilePath))
				{
					objFS = new FileStream(strFilePath, FileMode.Create);
				}
				else
					objFS = new FileStream(strFilePath, FileMode.Append);

				using (StreamWriter Sr = new StreamWriter(objFS))
				{
					Sr.WriteLine(String.Format("{0:HH:mm:ss}---{1}", System.DateTime.Now, text));
				}
			}
			catch (Exception ex)
			{
			}
		}

		public static void SaveNESLog(string text)
		{
			try
			{
				if (Directory.Exists(_BaselogPath + @"\Activity Log\NES\") == false)
					Directory.CreateDirectory(_BaselogPath + @"\Activity Log\NES\");

				FileStream objFS = null;
				string strFilePath = String.Format(@"{0}\Activity Log\NES\\{1:yyyy-MM-dd}.txt", _BaselogPath, System.DateTime.Now);
				if (!File.Exists(strFilePath))
				{
					objFS = new FileStream(strFilePath, FileMode.Create);
				}
				else
					objFS = new FileStream(strFilePath, FileMode.Append);

				using (StreamWriter Sr = new StreamWriter(objFS))
				{
					Sr.WriteLine(String.Format("{0:HH:mm:ss}---{1}", System.DateTime.Now, text));
				}
			}
			catch (Exception ex)
			{
			}
		}


		#endregion Public Methods
	}

	public class ExceptionArgs : EventArgs
	{
		public Exception ExceptionObj;
		public bool ShowDetailsBtn = true;
		public bool? ShowExceptionMsg = true;

		public ExceptionArgs(Exception pobjException, bool? pblnShowExceptionMsg)
		{
			ExceptionObj = pobjException;
			ShowExceptionMsg = pblnShowExceptionMsg;
		}

		public ExceptionArgs(Exception pobjException, bool? pblnShowExceptionMsg, bool pblnShowDetailsBtn)
		{
			ExceptionObj = pobjException;
			ShowExceptionMsg = pblnShowExceptionMsg;
			ShowDetailsBtn = pblnShowDetailsBtn;
		}
	}

	public class ExceptionForLogArgs : EventArgs
	{
		public string ExceptionMessage = string.Empty;
		public string FriendlyMessage = string.Empty;
		public Exception exception = null;

		public ExceptionForLogArgs(string friendlyMessage, string exceptionMessage, Exception ex)
		{
			this.ExceptionMessage = exceptionMessage;
			this.FriendlyMessage = friendlyMessage;
			this.exception = ex;
		}
	}
}
