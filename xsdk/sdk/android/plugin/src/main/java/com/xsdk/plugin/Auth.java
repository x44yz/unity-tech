package com.xsdk.plugin;

import com.xsdk.core.ResultAPI;

import org.json.JSONException;
import org.json.JSONObject;

/**
 * Created by twenty0ne on 2018/9/21.
 */

public class Auth {
    private ICallEngine callEngine;

    public Auth(ICallEngine callEngine) {
        this.callEngine = callEngine;
    }

    public String init(final String targetObject, final JSONObject jsonParam) {
        com.xsdk.core.Auth.init(new com.xsdk.core.Auth.AuthInitListener() {
            public void onAuthInit(ResultAPI result, com.xsdk.core.Auth.AuthInitResult authInitResult) {
                JSONObject resJsonParam = XPlugin.createResponse(result, jsonParam);
                try
                {
                    if (authInitResult == null) {
                        authInitResult = new com.xsdk.core.Auth.AuthInitResult();
                    }
                    resJsonParam.put("authInitResult", authInitResult.toJson());
                }
                catch (JSONException localJSONException) {}
                String resJsonParamString = resJsonParam.toString();
                Auth.this.callEngine.callEngine(targetObject, resJsonParamString);
            }
        });
        return "";
    }

    public String login(final String targetObject, final JSONObject jsonParam) {
        com.xsdk.core.Auth.login(new com.xsdk.core.Auth.AuthLoginListener(){
            public void onAuthLogin(ResultAPI result, com.xsdk.core.Auth.Account account) {
                JSONObject resJsonParam = XPlugin.createResponse(result, jsonParam);
                try
                {
                    if (account == null) {
                        resJsonParam.put("account", account.toJson());
                    }
                }
                catch (JSONException localJSONException) {}
                String resJsonParamString = resJsonParam.toString();
                Auth.this.callEngine.callEngine(targetObject, resJsonParamString);
            }
        });
        return "";
    }

    public String logout(final String targetObject, final JSONObject jsonParam) {
        com.xsdk.core.Auth.logout(new com.xsdk.core.Auth.AuthLogoutListener(){
            public void onAuthLogout(ResultAPI result) {
                JSONObject resJsonParam = XPlugin.createResponse(result, jsonParam);
                String resJsonParamString = resJsonParam.toString();
                Auth.this.callEngine.callEngine(targetObject, resJsonParamString);
            }
        });
        return "";
    }
}
