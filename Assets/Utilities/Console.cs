using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using TMPro;
using System;
using System.Linq;

public class Message
{
	public string text;
	public Color color;
	public TextMeshProUGUI openComponent;
	public TextMeshProUGUI closedComponent;

	public Message(string text, Color color, TextMeshProUGUI openComponent, TextMeshProUGUI closedComponent)
	{
		this.text = text;
		this.color = color;
		this.openComponent = openComponent;
		this.closedComponent = closedComponent;
	}
}

public class Console : MonoBehaviour
{

	private static Console instance;


	public static Message SendMessageToChat(string message)
	{
		return SendMessageToChat(message, Color.white);
	}

	public static Message SendMessageToChat(string message, Color color)
	{
		return instance.Send(message, color);
	}

	public static void RemoveMessageFromChat(Message message)
	{
		instance.Unsend(message);
	}

	[SerializeField] private TextMeshProUGUI textPrefab = null;
	[SerializeField] private RectTransform closedChatContent = null;
	[SerializeField] private RectTransform openChatContent = null;
	[SerializeField] private TMP_InputField inputField = null;

	bool active = false;
	bool pressedTab;
	List<string> autocompletedWords;
	int currentIndex;

	private void Awake()
	{
		if (instance == null)
		{
			DontDestroyOnLoad(gameObject);
			instance = this;
			commands = new Dictionary<string, Command>();
			messages = new List<Message>();
			var allCommands = Utilities.GetEnumerableOfType<Command>();
			foreach (var command in allCommands)
			{
				commands.Add(command.Name(), command);
			}
			inputField.onValueChanged.AddListener(InputField_OnValueChanged);
			SetChatActive(false);
		}
		else
		{
			Destroy(this);
		}
	}

	private void InputField_OnValueChanged(string value)
	{
		if(!pressedTab)
		{
			if(autoCompleteMessage != null)
			{
				Unsend(autoCompleteMessage);
				autocompletedWords = null;
			}
		}
	}

	private Message Send(string text, Color color)
	{
		if (messages.Count >= 25)
		{
			Unsend(messages[0]);
		}
		TextMeshProUGUI openComponent = Instantiate(textPrefab, openChatContent);
		openComponent.text = text;
		openComponent.color = color;
		TextMeshProUGUI closedComponent = Instantiate(openComponent, closedChatContent);
		Destroy(closedComponent.gameObject, 5);
		Message message = new Message(text, color, openComponent, closedComponent);
		messages.Add(message);
		return message;
	}

	private void Unsend(Message message)
	{
		if (messages.Contains(message))
		{
			Destroy(message.openComponent);
			if(message.closedComponent != null)
			{
				Destroy(message.closedComponent);
			}
			messages.Remove(message);
		}
	}

	Dictionary<string, Command> commands;
	List<Message> messages;

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(active)
			{
				SetChatActive(false);
			}
		}
		if (Input.GetKeyDown(KeyCode.Return))
		{
			if (active)
			{
				string word = inputField.text;
				if (word != "")
				{
					if(!TestCommand(word.Split(' ')))
					{
						SendMessageToChat(word);
					}
				}
				SetChatActive(false);
			}
			else
			{
				SetChatActive(true);
			}
		}
		if(Input.GetKeyDown(KeyCode.Tab))
		{
			if (active)
			{
				string word = inputField.text;
				AutoComplete(word.Split(' '));
			}
		}
	}

	private bool TestCommand(string[] words)
	{
		if (commands.TryGetValue(words[0], out Command value))
		{
			Argument currentArgument = value.Call();
			for (int i = 1; i < words.Length && currentArgument != null; i++)
			{
				currentArgument = currentArgument.NextArgument(words[i]);
			}
			if (currentArgument != null)
			{
				SendMessageToChat("Invalid arguments. Valid: '" + value.Usage() + "'", Color.red);
			}
		}
		else if (words[0][0] == '/')
		{
			SendMessageToChat("Unknown Command", Color.red);
		}
		else
		{
			return false;
		}
		return true;
	}

	Message autoCompleteMessage = null;

	private void AutoComplete(string[] words)
	{
		if (autocompletedWords == null || autocompletedWords.Count == 0)
		{
			int currentArgumentIndex = words.Length - 1;
			List<string> args = null;
			if (currentArgumentIndex == 0)
			{
				args = FindAllCompleted(commands.Keys, words[0]);
			}
			else
			{
				if (commands.TryGetValue(words[0], out Command value))
				{
					if (value.HasArguments)
					{
						Argument currentArgument = value.Call();
						int i;
						for (i = 1; i < currentArgumentIndex && currentArgument != null; i++)
						{
							if (!currentArgument.IsLast())
							{
								currentArgument = currentArgument.NextArgument(words[i]);
							}
							else
							{
								currentArgument = null;
								break;
							}
						}
						if (i == currentArgumentIndex)
						{
							if (currentArgument != null)
							{
								if (currentArgument.possibleArguments != null)
								{
									args = FindAllCompleted(currentArgument.possibleArguments?.Invoke(), words[currentArgumentIndex]);
								}
							}
						}
					}
				}
			}
			currentIndex = 0;
			autocompletedWords = args;
			if (args != null && args.Count != 0)
			{
				SendAutoCompletedMessageToChat(words);
			}
		}
		else
		{
			currentIndex++;
			currentIndex %= autocompletedWords.Count;
			SendAutoCompletedMessageToChat(words);
		}
	}

	private void SendAutoCompletedMessageToChat(string[] words)
	{
		AutoCompleteMessage();
		string newText = "";
		for (int i = 0; i < words.Length - 1; i++)
		{
			newText += words[i];
			newText += " ";
		}
		newText += autocompletedWords[currentIndex];
		pressedTab = true;
		inputField.text = newText;
		inputField.caretPosition = newText.Length;
		pressedTab = false;
	}

	private void AutoCompleteMessage()
	{
		if(autoCompleteMessage != null)
		{
			Unsend(autoCompleteMessage);
		}
		string autocompleted = "";
		for(int i = 0; i < autocompletedWords.Count; i++)
		{
			autocompleted += ColorUtilities.SetTextColor(autocompletedWords[i], i == currentIndex ? Color.green : Color.yellow) + " ";
		}
		autoCompleteMessage = SendMessageToChat(autocompleted);
	}

	public void SetChatActive(bool value)
	{
		inputField.gameObject.SetActive(value);
		active = value;
		openChatContent.gameObject.SetActive(value);
		closedChatContent.gameObject.SetActive(!value);
		inputField.text = "";
		if (value)
		{
			inputField.ActivateInputField();
		}
		else
		{
			inputField.DeactivateInputField();
		}
	}

	private List<string> FindAllCompleted(IEnumerable<string> allStrings, string start)
	{
		return new List<string>(allStrings).FindAll((string s) => s.StartsWith(start));
	}
}
