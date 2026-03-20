# Strict Email Assistant - Test Examples

This document shows how the **ULTRA-STRICT** email assistant will respond to various requests.

## 🚫 **NON-EMAIL REQUESTS (WILL BE REFUSED)**

### **Test Case 1: Jokes/Entertainment**
**Input:** "Tell me a joke"
**Expected Response:**
```json
{"error": "Sorry, I can't help you answer this question. I can only assist with email-related drafting tasks."}
```

### **Test Case 2: Math Problems**
**Input:** "What is 2 + 2?"
**Expected Response:**
```json
{"error": "Sorry, I can't help you answer this question. I can only assist with email-related drafting tasks."}
```

### **Test Case 3: General Knowledge**
**Input:** "What is Azure?"
**Expected Response:**
```json
{"error": "Sorry, I can't help you answer this question. I can only assist with email-related drafting tasks."}
```

### **Test Case 4: Technical Explanations**
**Input:** "Explain machine learning"
**Expected Response:**
```json
{"error": "Sorry, I can't help you answer this question. I can only assist with email-related drafting tasks."}
```

### **Test Case 5: Code Generation**
**Input:** "Write Python code for sorting"
**Expected Response:**
```json
{"error": "Sorry, I can't help you answer this question. I can only assist with email-related drafting tasks."}
```

### **Test Case 6: Weather/News**
**Input:** "What's the weather today?"
**Expected Response:**
```json
{"error": "Sorry, I can't help you answer this question. I can only assist with email-related drafting tasks."}
```

### **Test Case 7: Translations**
**Input:** "Translate 'hello' to Spanish"
**Expected Response:**
```json
{"error": "Sorry, I can't help you answer this question. I can only assist with email-related drafting tasks."}
```

### **Test Case 8: Stories/Creative Writing**
**Input:** "Write a story about a dragon"
**Expected Response:**
```json
{"error": "Sorry, I can't help you answer this question. I can only assist with email-related drafting tasks."}
```

---

## ✅ **VALID EMAIL REQUESTS (WILL BE ACCEPTED)**

### **Test Case 9: Professional Meeting Request**
**Input:** "Write a professional email to John about scheduling a meeting to discuss the Q4 budget"
**Expected Response:**
```json
{
  "subject": "Meeting Request - Q4 Budget Discussion",
  "greeting": "Dear John,",
  "body": "I hope this email finds you well. I would like to schedule a meeting with you to discuss the Q4 budget planning and review our financial projections for the upcoming quarter.\n\nWould you be available sometime next week for a 30-minute discussion? I'm flexible with timing and can accommodate your schedule.\n\nPlease let me know what works best for you, and I'll send a calendar invitation accordingly.",
  "closing": "Best regards,"
}
```

### **Test Case 10: Friendly Follow-up**
**Input:** "Create a friendly follow-up email to Sarah about the project timeline we discussed last week"
**Expected Response:**
```json
{
  "subject": "Follow-up: Project Timeline Discussion",
  "greeting": "Hi Sarah,",
  "body": "I wanted to follow up on our conversation last week regarding the project timeline. I've been thinking about the points you raised, and I believe we can make some adjustments to better align with the team's capacity.\n\nI've drafted a revised timeline that addresses the concerns we discussed. Would you like to review it together sometime this week?\n\nLooking forward to hearing your thoughts!",
  "closing": "Cheers,"
}
```

### **Test Case 11: Formal Leave Request**
**Input:** "Draft a formal leave request email to my manager for 3 days off next month for a family emergency"
**Expected Response:**
```json
{
  "subject": "Leave Request - Family Emergency",
  "greeting": "Dear [Manager Name],",
  "body": "I am writing to formally request leave from work for three days next month due to a family emergency that requires my immediate attention.\n\nI understand this is short notice, and I apologize for any inconvenience this may cause. I am committed to ensuring all my current projects are up to date before my departure and will coordinate with the team to minimize any disruption to our workflow.\n\nI would be happy to discuss this request further at your convenience. Thank you for your understanding and consideration.",
  "closing": "Sincerely,"
}
```

### **Test Case 12: Informal Thank You**
**Input:** "Write an informal email to thank Mike for helping with the presentation"
**Expected Response:**
```json
{
  "subject": "Thanks for Your Help!",
  "greeting": "Hey Mike,",
  "body": "Just wanted to drop you a quick note to say thanks for all your help with the presentation yesterday. Your insights really made a difference, and I couldn't have pulled it together without your support.\n\nThe client meeting went great, and they were particularly impressed with the data analysis section you helped me refine. I owe you one!",
  "closing": "Thanks,"
}
```

---

## 🎯 **KEY FEATURES OF THE STRICT PROMPT**

### **1. Zero Tolerance for Non-Email Requests**
- ❌ **Will NOT answer**: Jokes, math, explanations, code, weather, etc.
- ✅ **Will ONLY answer**: Email drafting requests

### **2. Consistent Error Response**
- **Format**: `{"error": "Sorry, I can't help you answer this question. I can only assist with email-related drafting tasks."}`
- **No exceptions**: Same message for ALL non-email requests

### **3. Structured Email Output**
- **Subject**: Concise, relevant to purpose
- **Greeting**: Tone-appropriate salutation ONLY
- **Body**: Main content WITHOUT greeting or closing
- **Closing**: Sign-off phrase ONLY

### **4. Tone Compliance**
- **Professional**: "Dear [Name]," + "Best regards,"
- **Friendly**: "Hi [Name]," + "Cheers,"
- **Formal**: "Dear [Title] [Name]," + "Sincerely,"
- **Informal**: "Hey [Name]," + "Thanks,"

### **5. Content Quality**
- ✅ **Contextual**: Matches email type and purpose
- ✅ **Natural**: Incorporates key points smoothly
- ✅ **Professional**: Maintains business standards
- ✅ **Unique**: No templates or generic content

---

## 🧪 **Testing Instructions**

### **To Test Non-Email Rejection:**
1. Try any of the refused examples above
2. Verify you get the exact error JSON
3. Confirm NO email content is generated

### **To Test Email Generation:**
1. Use any valid email request format
2. Verify you get proper JSON structure
3. Check that content matches tone and purpose
4. Ensure greeting/body/closing are properly separated

### **Expected Behavior:**
- 🚫 **100% rejection rate** for non-email requests
- ✅ **100% success rate** for valid email requests
- 📝 **Consistent JSON format** for all responses
- 🎯 **Contextual content** that matches user requirements

This strict prompt ensures your AI assistant will **NEVER** answer anything except email drafting requests!