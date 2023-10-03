mergeInto(LibraryManager.library,
{
	GetToken: function (key) {
    
		var token1 = localStorage[UTF8ToString(key)];
		//window.alert("get token : " + token1);
		if (!(token1 === undefined || token1  === null))
		{
			var len = lengthBytesUTF8(token1) + 1;
			var buffer = _malloc(len);
			stringToUTF8(token1, buffer, len);
	 
			return buffer;
		}
		else
		{
			return null;
		}
	},
	
	//HideLogs: function () {
    //	if(!window.console) window.console = {};
    //	var methods = ["log", "debug", "warn", "info"];
    //	for(var i=0;i<methods.length;i++)
	//	{
    //    console[methods[i]] = function(){};
    //    }
    //},
	
	
	//ResumeAudio: function(){
	//	var tryToResumeAudioContextBtnClick = function() {
	//			var count = new Number(document.getElementById("btnAudioCount").value);// in case of Number
	//			if (count <= 3){
	//				document.getElementById("btnAudio").click();
	//				document.getElementById("btnAudioCount").value=new Number((count+1));// in case of Number
	//			}
	//			else
	//				clearInterval(resumeIntervalBtnClick);
	//		};
	//		var resumeIntervalBtnClick = setInterval(tryToResumeAudioContextBtnClick, 400);
	//		window.alert("Resume Audio");
	//}
});


