

 #####  Tag Action Text  #####


It make your <Tag> with the Actions to use in a text.
I think that it is a general work to this function adding a link to the web site to a part of a text, similar to html. Nor is this all, you can connect your tag to anything your functions freely. It gives the actions to the text surrounded by <myTag> made by yourself with the same sense that word is bolded using the <b> Unity standard tag. Alternatively, it is also possible to register not tag but the character string itself and give the actions automatically on all corresponding character strings in a text.
It can be used the normal work of UGUI-Text or TextMeshPro(UI), so you can add the tag function while using the original normal work of them by simply replacing the component. The actions are conducted by Manager(s), so it is easy to change only the connected function in runtime with keep text sentences and tags. It is made with attention to click detection, so suppressing the often case that your users get irritated unintentional response by text link when doing another operation such as screen scrolling.



========== Example of Use ==========

- Make the link to the website in several parts of long text
- Card effect description in a card game or item description in a item synthesis game, when another related card/item name is in the sentence, emphasize that name and to display small window with the image and commentary when click there.
- A mystery solving game or a controversial game, clicking on various words in the testimony of the defendant, a hero considers it and advancing to the next event if choose contradictory word.
- A language learning game or a conversational heterosexual characters game, playing the voice when click on a conversational sentence.
etc.

This kind of implementation and modification can be done easily.
It might be useful even more in such cases to mix behaviors like the example in a text, or give actions while pouring a text contents from such as JSON at run time, change only the connected actions with a text as it is. This asset is particularly useful if you want to create such behavior on UGUI-Text and TextMeshPro can not be used for reasons such as multilingual and reduced data volume, it has difficulties with only the unity standard function. You will be able to do a variety of the text gimmicks that were laboriously difficult before.



========== How to Use ==========

All of this asset is in the TagActionText folder in Project window.


1) Put TaActiongManager object in the scene by selecting menu in UnityEditor, Tools > TagActionText > Create GameObject > TagActionManager. Or another way, attach TagActionManager.cs in the Script folder in Project window to easy to understand GameObject in the scene. Your tags are registered in Inspector window of the TagActionManager. Details of each item of TagActionManager are explained in the next column "Each Setting", so it is skipped here.


2) You want the UGUI-Text adding the tag function, using TagActionText component instead of it.Put TagActionText object in the scene by selecting menu in UnityEditor, Tools > TagActionText > Create GameObject > UGUI-Text. Or another way, attach TagActionText.cs to GameObject in the scene.
Or you want to instead of TextMeshPro(UI), using TagActionTMPGUI component.
In this case, put TagActionTMPGUI object in the scene by selecting menu in UnityEditor, Tools > TagActionText > Create GameObject > TextMeshProUGUI. Or another way, attach TagActionTMPUGUI.cs to GameObject in the scene.


3) Look at the Inspector window of 2), assign TagActionManager that set 1) in TagManager field. Although it is also possible to switch the TagManager and link other actions remaining the same text, be sure not to forget assign it. 


And all you have to do is use it just like normal text. The tag actions are automatically connected to the part of the text in the game scene by writing tags set to TagActionManager in the text sentence.


* Having inheritance from normal UGUI's class, so need Canvas, EventSystem, and so on which UGUI requires as well. And it also need RectTransform and CanvasRenderer on GameObject. 

* In such cases that writing text from within a script, use UGUITagActionText namespace.

* When this asset tag is further contained within other this asset tag range (nested), nested tag is not worked and treated as just a string. This is to prevent unnecessary processing power due to multiple loops depending on the tag form

* TextMeshPro(UI) is supported on unity2018.3 or higher, it was inported as standard on unity as an official version. And it used RichText implementation of the functions for parts not consider inheritance of TextMeshProUGUI, so there are some notes in TagActionTMPUGUI.
- RichText can not be changed to false.
- If you want to get the current text from the component, use the GetText() method.
- If you use together the tag of this asset and part of the tag of RichText of TextMeshPro(color, link), there may be cases interfere with each other and not work properly.



========== Each Setting ==========

A description of each item of TagActionManager.


Register a character string to be tag in the first string field.
For example, register "myTag" in there, it will be <myTag>.



[TagType]
Select descriptive type of your tag from drop down list.

Enclosure: have the action in the part of enclosed by the tag, and receive the argument string of the part of surrounded by the tag
Example) <Tag>xxx</Tag>, the part of xxx is taken as the argument

Addition: have the action in the part of enclosed by the tag, and receive the argument string described after = and surrounded by "" in the opening tag
Example) <Tag="xxx">abcde</Tag>, the part of xxx is taken as the argument(format close to html)

Self: have the action registered as the tag string itself, and receive the argument that string itself
Example) If it registered the string "unity", the action is automatically given the part of all "unity" word in the text sentence without surrounded tags, and the argument also "unity" string

* If TagType is Enclosure or Addition, only used single-byte alphanumeric characters as tag character string.



[Text Color]
By checked and set the color, the text within the tag effect range becomes its color.
When uncheck it that text remain the same color as other normal text but have the action.



[Action]
It is UnityEvent to register the functions receive string and Vector2.
The received string is an argument set in the TagType item, and Vector2 is the ScreenPoint click event occurred.
(The registered functions must to takes the arguments for simplification of Inspector as a generic asset. Describe the functions in such a way as to receive string and Vector2 even if in case of you do not need it. See TagActionExample.cs in the Demo scene as an example.)

! Inpotant!
If you receive arguments from tags, select in the "Dynamic" part in the dropdown for registering the function.



[Tapped Text Color]
By checked and set the color, the text within the tag effect range becomes its color after being clicked.



===============================================================


created by: Myouji
myoujing4@gmail.com

Some resources of the Demo are used on the following license.
http://unity-chan.com/contents/license_en/

In creating this asset, I referred to the following on the MIT license. Thank you very much.
https://github.com/setchi/uGUI-Hypertext


