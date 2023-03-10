using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Prompt
{
    [JsonProperty("prompt")]
    public string PromptText { get; set; }
}

public class ImageData
{
    [JsonProperty("status")]
    public int Status;

    [JsonProperty("image_url")]
    public string ImageURL;
}

public class Message
{
    [JsonProperty("role")]
    public string Role { get; set; }

    [JsonProperty("content")]
    public string Content { get; set; }
}

public class Chat
{
    [JsonProperty("model")]
    public string Model { get; set; }

    [JsonProperty("messages")]
    public List<Message> Messages { get; set; }
}

public class Choice
{
    [JsonProperty("index")]
    public int Index { get; set; }

    [JsonProperty("finish_reason")]
    public string FinishReason { get; set; }

    [JsonProperty("message")]
    public Message Message { get; set; }
}

public class Usage
{
    [JsonProperty("prompt_tokens")]
    public int PromptTokens { get; set; }

    [JsonProperty("completion_tokens")]
    public int CompletionTokens { get; set; }

    [JsonProperty("total_tokens")]
    public int TotalTokens { get; set; }
}

public class ChatResponse
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("object")]
    public string Object { get; set; }

    [JsonProperty("created")]
    public int Created { get; set; }

    [JsonProperty("model")]
    public string Model { get; set; }

    [JsonProperty("choices")]
    public List<Choice> Choices { get; set; }

    [JsonProperty("usage")]
    public Usage Usage { get; set; }
}

public class GameData
{
    [JsonProperty("map_image")]
    public string MapImage { get; set; }

    [JsonProperty("background_story")]
    public string BackgroundStory { get; set; }
}

public class CharacterData
{
    [JsonProperty("character_image")]
    public string CharacterImage { get; set; }

    [JsonProperty("character_story")]
    public string CharacterStory { get; set; }
}

