# Debug Test Guide for Ultra-Strict Email Assistant

## What We Fixed

1. **Enhanced System Prompt**: Updated to specifically detect non-email content in key points
2. **Improved Error Detection**: Added more comprehensive error detection patterns in AIService
3. **Added Debug Logging**: Added console logging throughout the pipeline to trace what's happening

## Test Cases to Try

### Test Case 1: Math Question in Key Points
- **Recipient Name**: John Doe
- **Tone**: Professional  
- **Email Type**: FollowUp
- **Key Points**: What is 2+2?

**Expected Result**: Error message "Sorry, I can't help you answer this question. I can only assist with email-related drafting tasks."

### Test Case 2: Joke Request in Key Points
- **Recipient Name**: Sarah Smith
- **Tone**: Friendly
- **Email Type**: MeetingRequest  
- **Key Points**: Tell me a joke

**Expected Result**: Error message "Sorry, I can't help you answer this question. I can only assist with email-related drafting tasks."

### Test Case 3: Weather Question in Key Points
- **Recipient Name**: Mike Johnson
- **Tone**: Formal
- **Email Type**: LeaveRequest
- **Key Points**: What's the weather like today?

**Expected Result**: Error message "Sorry, I can't help you answer this question. I can only assist with email-related drafting tasks."

### Test Case 4: Valid Email Request (Should Work)
- **Recipient Name**: Alice Brown
- **Tone**: Professional
- **Email Type**: MeetingRequest
- **Key Points**: Schedule a meeting for next Tuesday at 2 PM to discuss the quarterly budget review

**Expected Result**: Generated email with proper subject, greeting, body, and closing

## How to Test

1. **Start both applications**:
   ```bash
   # Terminal 1 - Backend
   cd /path/to/Email_Assistant
   dotnet run
   
   # Terminal 2 - Frontend  
   cd /path/to/EmailAssistant.UI
   dotnet run
   ```

2. **Open browser** to the frontend URL (usually http://localhost:5010)

3. **Test each case** by:
   - Clicking "Generate an Email" button
   - Filling in the form with test case data
   - Clicking "Generate Email"
   - Observing the result

## Debug Information to Look For

### In Backend Console (Terminal 1):
```
DEBUG: AI Response Content: '[AI response text]'
DEBUG: Finish Reason: [ToolCalls/Stop/etc]
DEBUG: Tool Calls Count: [number]
DEBUG: Detected refusal in AI response - throwing exception (if error detected)
DEBUG: Backend exception - [error message] (if exception thrown)
```

### In Frontend Console (Browser Developer Tools):
```
DEBUG: Sending request to backend - RecipientName: [name], KeyPoints: [points]
DEBUG: Backend response status: [200/400/500]
DEBUG: Backend error content: '[error message]' (if error)
DEBUG: Exception caught in GenerateEmail: [error message] (if error)
```

## What to Report Back

Please test the cases above and report:

1. **Which test cases work correctly** (show error message)
2. **Which test cases fail** (generate email instead of error)
3. **Console output** from both backend and frontend for failing cases
4. **Screenshots** of the UI showing the result

This will help us identify exactly where the issue is occurring in the pipeline.

## Troubleshooting

If you see:
- **No debug output**: Check that both applications are running
- **"Unable to connect to server"**: Check CORS configuration and ports
- **Email generated instead of error**: The AI is not following the strict prompt - we'll need to adjust it further
- **Error not showing in UI**: There's an issue in the frontend error handling

## Next Steps

Based on your test results, we can:
1. Further refine the system prompt if AI isn't refusing properly
2. Fix frontend error display if errors aren't showing
3. Adjust error detection patterns if needed
4. Remove debug logging once everything works correctly