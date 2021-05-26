using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KeyboardMan2D
{
    public class UIDialog : MonoBehaviour
    {
        public static UIDialog instance;

        private void Awake()
        {
            if (instance)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }

        public Image avatarLeft;
        public Image avatarRight;

        public Text textSpeakerLeft;
        public Text textSpeakerRight;

        public UIDialogContent dialogContent;
        public Text textDialogContentTip;

        public UIDialogSelections dialogSelections;

        /// <summary>
        /// 正在进行一个对话
        /// </summary>
        public bool Speaking { get; private set; } = false;

        private List<Paragraph> playedParagraphs = new List<Paragraph>();

        /// <summary>
        /// 当前对话
        /// </summary>
        private Dialog dialog;

        /// <summary>
        /// 段落指针
        /// </summary>
        private int paragraphPtr = 0;

        public void Initialize()
        {
            playedParagraphs = new List<Paragraph>();
            EndDialog();
        }

        /// <summary>
        /// 设置对话，在调用StartDialog之后即可开始演绎此对话
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="speaker"></param>
        public void SetDialog(Dialog dialog)
        {
            this.dialog = dialog;

            if(dialog)
            {
                Speaker speaker = Speaker.FindSpeakerWithObjectName(dialog.speaker);
                if (speaker)
                {
                    speaker.SetDialog(dialog);
                }
            }
        }

        /// <summary>
        /// 开始演绎对话
        /// </summary>
        public void StartDialog()
        {
            Speaking = true;

            textDialogContentTip.text = "单击\"Space\"以继续";

            if (!dialog || dialog.paragraphs.Length <= 0)
            {
                EndDialog();
                return;
            }

            paragraphPtr = 0;
            ShowParagraph();
        }

        /// <summary>
        /// 跳转至下一段落
        /// </summary>
        public void NextParagraph()
        {
            if (!dialog || GameManager.instance.Paused || !Speaking)
            {
                return;
            }

            paragraphPtr++;
            if (paragraphPtr == dialog.paragraphs.Length)
            {
                ShowSelections();
            }
            else if (paragraphPtr < dialog.paragraphs.Length)
            {
                ShowParagraph();
            }
        }

        /// <summary>
        /// 跳转至上一段落
        /// </summary>
        public void PrevParagraph()
        {
            if (!dialog || GameManager.instance.Paused || !Speaking)
            {
                return;
            }

            paragraphPtr--;
            if(paragraphPtr >= 0)
            {
                ShowParagraph();
            }
            else
            {
                paragraphPtr = 0;
            }
        }

        /// <summary>
        /// 演绎段落
        /// </summary>
        private void ShowParagraph()
        {
            // 隐藏选项
            dialogSelections.gameObject.SetActive(false);

            Paragraph paragraph = dialog.paragraphs[paragraphPtr];

            // 显示讲述人头像及名称
            Sprite sprite = avatarLeft.sprite = SpriteDictionary.instance.FindSpriteWithName(paragraph.speaker + "_1024");
            if (paragraph.spriteLocation == DialogSpriteLocation.Left)
            {
                avatarLeft.gameObject.SetActive(true);
                avatarLeft.sprite = sprite;
                avatarRight.gameObject.SetActive(false);

                textSpeakerLeft.text = paragraph.speaker.Split('-')[0];
            }
            else if (paragraph.spriteLocation == DialogSpriteLocation.Right)
            {
                avatarRight.gameObject.SetActive(true);
                avatarRight.sprite = sprite;
                avatarLeft.gameObject.SetActive(false);

                textSpeakerRight.text = paragraph.speaker.Split('-')[0];
            }

            // 显示对话框及段落内容
            dialogContent.gameObject.SetActive(true);
            dialogContent.SetText(paragraph.content);

            // 段落事件触发
            if (paragraph.singleEvent)
            {
                if(!playedParagraphs.Contains(paragraph))
                {
                    playedParagraphs.Add(paragraph);
                    paragraph.onSpeak.Invoke();
                }
            }
            else
            {
                paragraph.onSpeak.Invoke();
            }
        }

        /// <summary>
        /// 显示选项
        /// </summary>
        private void ShowSelections()
        {
            textDialogContentTip.text = "选择一个选项";

            if (dialog.selections.Length<=0)
            {
                EndDialog();
                return;
            }

            dialogSelections.SetSelections(dialog);
            dialogSelections.gameObject.SetActive(true);
        }

        /// <summary>
        /// 终止对话
        /// </summary>
        public void EndDialog()
        {
            Speaking = false;

            paragraphPtr = 0;
            dialog = null;

            avatarLeft.gameObject.SetActive(false);
            avatarRight.gameObject.SetActive(false);
            dialogContent.gameObject.SetActive(false);
            dialogSelections.gameObject.SetActive(false);
        }

        private void Update()
        {
            PressNextParagraph();
        }

        /// <summary>
        /// 按键-跳转至下一段落
        /// </summary>
        private void PressNextParagraph()
        {
            if (
                Input.GetKeyDown(KeyCode.Space) ||
                Input.GetKeyDown(KeyCode.Mouse0) ||
                Input.GetKeyDown(KeyCode.Mouse1) ||
                Input.GetKeyDown(KeyCode.Return)
                )
            {
                NextParagraph();
            }
        }
    }
}
