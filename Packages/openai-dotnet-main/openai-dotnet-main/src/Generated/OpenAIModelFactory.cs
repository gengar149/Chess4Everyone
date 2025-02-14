// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using OpenAI.Assistants;
using OpenAI.Audio;
using OpenAI.Chat;
using OpenAI.Embeddings;
using OpenAI.Images;
using OpenAI.Moderations;
using OpenAI.RealtimeConversation;
using OpenAI.VectorStores;

namespace OpenAI
{
    internal static partial class OpenAIModelFactory
    {
        public static VectorStoreFileCounts VectorStoreFileCounts(int inProgress = default, int completed = default, int failed = default, int cancelled = default, int total = default)
        {
            return new VectorStoreFileCounts(
                inProgress,
                completed,
                failed,
                cancelled,
                total,
                serializedAdditionalRawData: null);
        }

        public static VectorStoreFileAssociationError VectorStoreFileAssociationError(VectorStoreFileAssociationErrorCode code = default, string message = null)
        {
            return new VectorStoreFileAssociationError(code, message, serializedAdditionalRawData: null);
        }

        public static RunError RunError(RunErrorCode code = default, string message = null)
        {
            return new RunError(code, message, serializedAdditionalRawData: null);
        }

        public static RunIncompleteDetails RunIncompleteDetails(RunIncompleteReason? reason = null)
        {
            return new RunIncompleteDetails(reason, serializedAdditionalRawData: null);
        }

        public static RunTokenUsage RunTokenUsage(int outputTokenCount = default, int inputTokenCount = default, int totalTokenCount = default)
        {
            return new RunTokenUsage(outputTokenCount, inputTokenCount, totalTokenCount, serializedAdditionalRawData: null);
        }

        public static RunStepToolCall RunStepToolCall(string id = null)
        {
            return new UnknownRunStepDetailsToolCallsObjectToolCallsObject(default, id, serializedAdditionalRawData: null);
        }

        public static RunStepFileSearchResult RunStepFileSearchResult(string fileId = null, string fileName = null, float score = default, IEnumerable<RunStepFileSearchResultContent> content = null)
        {
            content ??= new List<RunStepFileSearchResultContent>();

            return new RunStepFileSearchResult(fileId, fileName, score, content?.ToList(), serializedAdditionalRawData: null);
        }

        public static RunStepFileSearchResultContent RunStepFileSearchResultContent(RunStepFileSearchResultContentKind kind = default, string text = null)
        {
            return new RunStepFileSearchResultContent(kind, text, serializedAdditionalRawData: null);
        }

        public static RunStepError RunStepError(RunStepErrorCode code = default, string message = null)
        {
            return new RunStepError(code, message, serializedAdditionalRawData: null);
        }

        public static RunStepTokenUsage RunStepTokenUsage(int outputTokenCount = default, int inputTokenCount = default, int totalTokenCount = default)
        {
            return new RunStepTokenUsage(outputTokenCount, inputTokenCount, totalTokenCount, serializedAdditionalRawData: null);
        }

        public static ConversationUpdate ConversationUpdate(string eventId = null)
        {
            return new UnknownRealtimeServerEvent(default, eventId, serializedAdditionalRawData: null);
        }

        public static ConversationInputAudioCommittedUpdate ConversationInputAudioCommittedUpdate(string eventId = null, string previousItemId = null, string itemId = null)
        {
            return new ConversationInputAudioCommittedUpdate(ConversationUpdateKind.InputAudioCommitted, eventId, serializedAdditionalRawData: null, previousItemId, itemId);
        }

        public static ConversationInputAudioClearedUpdate ConversationInputAudioClearedUpdate(string eventId = null)
        {
            return new ConversationInputAudioClearedUpdate(ConversationUpdateKind.InputAudioCleared, eventId, serializedAdditionalRawData: null);
        }

        public static ConversationInputTranscriptionFinishedUpdate ConversationInputTranscriptionFinishedUpdate(string eventId = null, string itemId = null, int contentIndex = default, string transcript = null)
        {
            return new ConversationInputTranscriptionFinishedUpdate(
                ConversationUpdateKind.InputTranscriptionFinished,
                eventId,
                serializedAdditionalRawData: null,
                itemId,
                contentIndex,
                transcript);
        }

        public static ConversationItemTruncatedUpdate ConversationItemTruncatedUpdate(string eventId = null, string itemId = null, int contentIndex = default, int audioEndMs = default)
        {
            return new ConversationItemTruncatedUpdate(
                ConversationUpdateKind.ItemTruncated,
                eventId,
                serializedAdditionalRawData: null,
                itemId,
                contentIndex,
                audioEndMs);
        }

        public static ConversationItemDeletedUpdate ConversationItemDeletedUpdate(string eventId = null, string itemId = null)
        {
            return new ConversationItemDeletedUpdate(ConversationUpdateKind.ItemDeleted, eventId, serializedAdditionalRawData: null, itemId);
        }

        public static ConversationTokenUsage ConversationTokenUsage(int totalTokens = default, int inputTokens = default, int outputTokens = default, ConversationInputTokenUsageDetails inputTokenDetails = null, ConversationOutputTokenUsageDetails outputTokenDetails = null)
        {
            return new ConversationTokenUsage(
                totalTokens,
                inputTokens,
                outputTokens,
                inputTokenDetails,
                outputTokenDetails,
                serializedAdditionalRawData: null);
        }

        public static ConversationInputTokenUsageDetails ConversationInputTokenUsageDetails(int cachedTokens = default, int textTokens = default, int audioTokens = default)
        {
            return new ConversationInputTokenUsageDetails(cachedTokens, textTokens, audioTokens, serializedAdditionalRawData: null);
        }

        public static ConversationOutputTokenUsageDetails ConversationOutputTokenUsageDetails(int textTokens = default, int audioTokens = default)
        {
            return new ConversationOutputTokenUsageDetails(textTokens, audioTokens, serializedAdditionalRawData: null);
        }

        public static ConversationItemStreamingTextFinishedUpdate ConversationItemStreamingTextFinishedUpdate(string eventId = null, string responseId = null, string itemId = null, int outputIndex = default, int contentIndex = default, string text = null)
        {
            return new ConversationItemStreamingTextFinishedUpdate(
                ConversationUpdateKind.ItemStreamingPartTextFinished,
                eventId,
                serializedAdditionalRawData: null,
                responseId,
                itemId,
                outputIndex,
                contentIndex,
                text);
        }

        public static ConversationItemStreamingAudioTranscriptionFinishedUpdate ConversationItemStreamingAudioTranscriptionFinishedUpdate(string eventId = null, string responseId = null, string itemId = null, int outputIndex = default, int contentIndex = default, string transcript = null)
        {
            return new ConversationItemStreamingAudioTranscriptionFinishedUpdate(
                ConversationUpdateKind.ItemStreamingPartAudioTranscriptionFinished,
                eventId,
                serializedAdditionalRawData: null,
                responseId,
                itemId,
                outputIndex,
                contentIndex,
                transcript);
        }

        public static ConversationItemStreamingAudioFinishedUpdate ConversationItemStreamingAudioFinishedUpdate(string eventId = null, string responseId = null, string itemId = null, int outputIndex = default, int contentIndex = default)
        {
            return new ConversationItemStreamingAudioFinishedUpdate(
                ConversationUpdateKind.ItemStreamingPartAudioFinished,
                eventId,
                serializedAdditionalRawData: null,
                responseId,
                itemId,
                outputIndex,
                contentIndex);
        }

        public static ConversationRateLimitsUpdate ConversationRateLimitsUpdate(string eventId = null, IEnumerable<ConversationRateLimitDetailsItem> allDetails = null)
        {
            allDetails ??= new List<ConversationRateLimitDetailsItem>();

            return new ConversationRateLimitsUpdate(ConversationUpdateKind.RateLimitsUpdated, eventId, serializedAdditionalRawData: null, allDetails?.ToList());
        }

        public static ConversationRateLimitDetailsItem ConversationRateLimitDetailsItem(string name = null, int maximumCount = default, int remainingCount = default, TimeSpan timeUntilReset = default)
        {
            return new ConversationRateLimitDetailsItem(name, maximumCount, remainingCount, timeUntilReset, serializedAdditionalRawData: null);
        }

        public static ModerationResultCollection ModerationResultCollection(string id = null, string model = null, IEnumerable<ModerationResult> results = null)
        {
            results ??= new List<ModerationResult>();

            return new ModerationResultCollection(id, model, results?.ToList());
        }

        public static MessageFailureDetails MessageFailureDetails(MessageFailureReason reason = default)
        {
            return new MessageFailureDetails(reason, serializedAdditionalRawData: null);
        }

        public static GeneratedImageCollection GeneratedImageCollection(DateTimeOffset created = default, IEnumerable<GeneratedImage> data = null)
        {
            data ??= new List<GeneratedImage>();

            return new GeneratedImageCollection(created, data?.ToList());
        }

        public static GeneratedImage GeneratedImage(BinaryData imageBytes = null, Uri imageUri = null, string revisedPrompt = null)
        {
            return new GeneratedImage(imageBytes, imageUri, revisedPrompt, serializedAdditionalRawData: null);
        }

        public static EmbeddingTokenUsage EmbeddingTokenUsage(int inputTokenCount = default, int totalTokenCount = default)
        {
            return new EmbeddingTokenUsage(inputTokenCount, totalTokenCount, serializedAdditionalRawData: null);
        }

        public static ChatTokenUsage ChatTokenUsage(int outputTokenCount = default, int inputTokenCount = default, int totalTokenCount = default, ChatOutputTokenUsageDetails outputTokenDetails = null, ChatInputTokenUsageDetails inputTokenDetails = null)
        {
            return new ChatTokenUsage(
                outputTokenCount,
                inputTokenCount,
                totalTokenCount,
                outputTokenDetails,
                inputTokenDetails,
                serializedAdditionalRawData: null);
        }

        public static ChatOutputTokenUsageDetails ChatOutputTokenUsageDetails(int audioTokenCount = default, int reasoningTokenCount = default)
        {
            return new ChatOutputTokenUsageDetails(audioTokenCount, reasoningTokenCount, serializedAdditionalRawData: null);
        }

        public static ChatInputTokenUsageDetails ChatInputTokenUsageDetails(int audioTokenCount = default, int cachedTokenCount = default)
        {
            return new ChatInputTokenUsageDetails(audioTokenCount, cachedTokenCount, serializedAdditionalRawData: null);
        }

        public static ChatMessage ChatMessage(ChatMessageContent content = null)
        {
            return new InternalUnknownChatMessage(default, content, serializedAdditionalRawData: null);
        }

        public static SystemChatMessage SystemChatMessage(ChatMessageContent content = null, string participantName = null)
        {
            return new SystemChatMessage(ChatMessageRole.System, content, serializedAdditionalRawData: null, participantName);
        }

        public static UserChatMessage UserChatMessage(ChatMessageContent content = null, string participantName = null)
        {
            return new UserChatMessage(ChatMessageRole.User, content, serializedAdditionalRawData: null, participantName);
        }

        public static AssistantChatMessage AssistantChatMessage(ChatMessageContent content = null, string refusal = null, string participantName = null, IEnumerable<ChatToolCall> toolCalls = null, ChatFunctionCall functionCall = null)
        {
            toolCalls ??= new List<ChatToolCall>();

            return new AssistantChatMessage(
                ChatMessageRole.Assistant,
                content,
                serializedAdditionalRawData: null,
                refusal,
                participantName,
                toolCalls?.ToList(),
                functionCall);
        }

        public static ToolChatMessage ToolChatMessage(ChatMessageContent content = null, string toolCallId = null)
        {
            return new ToolChatMessage(ChatMessageRole.Tool, content, serializedAdditionalRawData: null, toolCallId);
        }

        public static FunctionChatMessage FunctionChatMessage(ChatMessageContent content = null, string functionName = null)
        {
            return new FunctionChatMessage(ChatMessageRole.Function, content, serializedAdditionalRawData: null, functionName);
        }

        public static ChatFunction ChatFunction(string functionDescription = null, string functionName = null, BinaryData functionParameters = null)
        {
            return new ChatFunction(functionDescription, functionName, functionParameters, serializedAdditionalRawData: null);
        }

        public static ChatTokenLogProbabilityDetails ChatTokenLogProbabilityDetails(string token = null, float logProbability = default, ReadOnlyMemory<byte>? utf8Bytes = null, IEnumerable<ChatTokenTopLogProbabilityDetails> topLogProbabilities = null)
        {
            topLogProbabilities ??= new List<ChatTokenTopLogProbabilityDetails>();

            return new ChatTokenLogProbabilityDetails(token, logProbability, utf8Bytes, topLogProbabilities?.ToList(), serializedAdditionalRawData: null);
        }

        public static ChatTokenTopLogProbabilityDetails ChatTokenTopLogProbabilityDetails(string token = null, float logProbability = default, ReadOnlyMemory<byte>? utf8Bytes = null)
        {
            return new ChatTokenTopLogProbabilityDetails(token, logProbability, utf8Bytes, serializedAdditionalRawData: null);
        }

        public static TranscribedWord TranscribedWord(string word = null, TimeSpan startTime = default, TimeSpan endTime = default)
        {
            return new TranscribedWord(word, startTime, endTime, serializedAdditionalRawData: null);
        }

        public static TranscribedSegment TranscribedSegment(int id = default, int seekOffset = default, TimeSpan startTime = default, TimeSpan endTime = default, string text = null, ReadOnlyMemory<int> tokenIds = default, float temperature = default, float averageLogProbability = default, float compressionRatio = default, float noSpeechProbability = default)
        {
            return new TranscribedSegment(
                id,
                seekOffset,
                startTime,
                endTime,
                text,
                tokenIds,
                temperature,
                averageLogProbability,
                compressionRatio,
                noSpeechProbability,
                serializedAdditionalRawData: null);
        }

        public static StreamingChatFunctionCallUpdate StreamingChatFunctionCallUpdate(string functionName = null, BinaryData functionArgumentsUpdate = null)
        {
            return new StreamingChatFunctionCallUpdate(functionName, functionArgumentsUpdate, serializedAdditionalRawData: null);
        }
    }
}
