using KindWords.Models;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text;

namespace KindWords.Services
{
    /// <summary>
    /// Service for managing messages and replies via REST API
    /// </summary>
    public class MessageService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationService _authService;
        private const string BaseUrl = "https://localhost:7001/api";

        public MessageService(IHttpClientFactory httpClientFactory, AuthenticationService authService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _authService = authService;
        }

        /// <summary>
        /// Get random messages for inbox that the current user hasn't replied to yet
        /// </summary>
        public async Task<List<Message>> GetInboxMessagesAsync(int count = 5)
        {
            try
            {
                if (!AddAuthorizationHeader())
                {
                    System.Diagnostics.Debug.WriteLine("Not authenticated - cannot get inbox messages");
                    return new List<Message>();
                }

                var response = await _httpClient.GetAsync($"{BaseUrl}/messages/inbox?count={count}");
                
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var messageDtos = JsonSerializer.Deserialize<List<MessageDto>>(json, GetJsonOptions());
                    return messageDtos?.Select(ConvertFromDto).ToList() ?? new List<Message>();
                }
                
                return new List<Message>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting inbox messages: {ex.Message}");
                return new List<Message>();
            }
        }

        /// <summary>
        /// Get current user's messages with all their replies (only visible to message creator)
        /// </summary>
        public async Task<List<Message>> GetMyMessagesAsync()
        {
            try
            {
                if (!AddAuthorizationHeader())
                {
                    System.Diagnostics.Debug.WriteLine("Not authenticated - cannot get my messages");
                    return new List<Message>();
                }

                var response = await _httpClient.GetAsync($"{BaseUrl}/messages/my-messages");
                
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var messageDtos = JsonSerializer.Deserialize<List<MessageDto>>(json, GetJsonOptions());
                    return messageDtos?.Select(ConvertFromDto).ToList() ?? new List<Message>();
                }
                
                return new List<Message>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting my messages: {ex.Message}");
                return new List<Message>();
            }
        }

        public async Task<List<Reply>> GetRepliesForMessageAsync(int messageId)
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.GetAsync($"{BaseUrl}/messages/{messageId}");
                
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var messageDto = JsonSerializer.Deserialize<MessageDto>(json, GetJsonOptions());
                    return messageDto?.Replies?.Select(ConvertReplyFromDto).ToList() ?? new List<Reply>();
                }
                
                return new List<Reply>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting replies for message {messageId}: {ex.Message}");
                return new List<Reply>();
            }
        }

        public async Task<Message> SendMessageAsync(string content, string category)
        {
            try
            {
                if (!AddAuthorizationHeader())
                {
                    System.Diagnostics.Debug.WriteLine("Not authenticated - cannot send message");
                    return new Message();
                }
                
                var request = new CreateMessageRequest
                {
                    Content = content,
                    Category = category
                };
                
                var json = JsonSerializer.Serialize(request, GetJsonOptions());
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync($"{BaseUrl}/messages", httpContent);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Message sent successfully: {responseJson}");
                    var messageDto = JsonSerializer.Deserialize<MessageDto>(responseJson, GetJsonOptions());
                    return messageDto != null ? ConvertFromDto(messageDto) : new Message();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Failed to send message. Status: {response.StatusCode}, Content: {errorContent}");
                }
                
                return new Message();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error sending message: {ex.Message}");
                return new Message();
            }
        }

        public async Task<Reply> SendReplyAsync(int messageId, string content)
        {
            try
            {
                AddAuthorizationHeader();
                
                var request = new CreateReplyRequest
                {
                    MessageId = messageId,
                    Content = content
                };
                
                var json = JsonSerializer.Serialize(request, GetJsonOptions());
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync($"{BaseUrl}/messages/{messageId}/reply", httpContent);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var replyDto = JsonSerializer.Deserialize<ReplyDto>(responseJson, GetJsonOptions());
                    return replyDto != null ? ConvertReplyFromDto(replyDto) : new Reply();
                }
                
                return new Reply();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error sending reply: {ex.Message}");
                return new Reply();
            }
        }

        /// <summary>
        /// Search messages in inbox (only messages user hasn't replied to)
        /// </summary>
        public async Task<List<Message>> SearchInboxMessagesAsync(string searchTerm, string? category = null)
        {
            try
            {
                AddAuthorizationHeader();
                var url = $"{BaseUrl}/messages/search?term={Uri.EscapeDataString(searchTerm)}";
                if (!string.IsNullOrEmpty(category) && category != "All")
                {
                    url += $"&category={Uri.EscapeDataString(category)}";
                }
                
                var response = await _httpClient.GetAsync(url);
                
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var messageDtos = JsonSerializer.Deserialize<List<MessageDto>>(json, GetJsonOptions());
                    return messageDtos?.Select(ConvertFromDto).ToList() ?? new List<Message>();
                }
                
                return new List<Message>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error searching messages: {ex.Message}");
                return new List<Message>();
            }
        }

        #region Helper Methods

        private bool AddAuthorizationHeader()
        {
            var token = _authService.CurrentUser?.Token;
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                return true;
            }
            return false;
        }

        private JsonSerializerOptions GetJsonOptions()
        {
            return new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        private Message ConvertFromDto(MessageDto dto)
        {
            return new Message
            {
                Id = dto.Id,
                Content = dto.Content,
                Category = dto.Category,
                CreatedAt = dto.CreatedAt,
                UserId = dto.UserId,
                IsAnonymous = dto.IsAnonymous,
                ReplyCount = dto.ReplyCount,
                HasBeenRepliedTo = dto.HasBeenRepliedTo,
                RepliedByUserIds = dto.RepliedByUserIds,
                AllReplies = dto.Replies?.Select(ConvertReplyFromDto).ToList() ?? new List<Reply>()
            };
        }

        private Reply ConvertReplyFromDto(ReplyDto dto)
        {
            return new Reply
            {
                Id = dto.Id,
                MessageId = dto.MessageId,
                Content = dto.Content,
                CreatedAt = dto.CreatedAt,
                UserId = dto.UserId,
                IsAnonymous = dto.IsAnonymous
            };
        }

        #endregion
    }

    // DTO classes for API communication
    public class CreateMessageRequest
    {
        public string Content { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }

    public class CreateReplyRequest
    {
        public int MessageId { get; set; }
        public string Content { get; set; } = string.Empty;
    }

    public class MessageDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public bool IsAnonymous { get; set; }
        public int ReplyCount { get; set; }
        public bool HasBeenRepliedTo { get; set; }
        public List<ReplyDto> Replies { get; set; } = new();
        public List<Guid> RepliedByUserIds { get; set; } = new();
    }

    public class ReplyDto
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public bool IsAnonymous { get; set; }
    }
} 