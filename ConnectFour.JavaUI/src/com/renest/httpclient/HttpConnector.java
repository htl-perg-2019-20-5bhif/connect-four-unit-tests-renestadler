package com.renest.httpclient;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import org.apache.http.Header;
import org.apache.http.HttpEntity;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.methods.*;
import org.apache.http.entity.ContentType;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.CloseableHttpClient;
import org.apache.http.impl.client.HttpClients;
import org.apache.http.util.EntityUtils;

import java.io.IOException;
import java.lang.reflect.Type;
import java.time.LocalDate;
import java.time.LocalDateTime;

public class HttpConnector {
    Gson gs;
    private String baseUrl;
    private CloseableHttpClient client;

    protected HttpConnector(String baseUrl) {
        if (baseUrl == null) {
            this.baseUrl = "";
        } else {
            this.baseUrl = baseUrl;
        }
        this.client = HttpClients.createDefault();
        gs = new GsonBuilder()
                .setPrettyPrinting()
                .registerTypeAdapter(LocalDate.class, new LocalDateAdapter())
                .registerTypeAdapter(LocalDateTime.class, new LocalDateTimeAdapter())
                .create();
    }

    protected <T> HttpResponse<T> doGet(String path, Header[] headers, Type type) {
        HttpGet httpGet = new HttpGet(baseUrl + "/" + path);
        if (headers != null) {
            httpGet.setHeaders(headers);
        }
        return manageRequest(httpGet, type);
    }

    protected <T> HttpResponse<T> doPost(String path, Header[] headers, T object, Type type) {
        HttpPost httpPost = new HttpPost(baseUrl + "/" + path);
        if (headers != null) {
            httpPost.setHeaders(headers);
        }
        if(object==null){
            return manageRequest(httpPost, type);
        }
        StringEntity body = new StringEntity(
                gs.toJson(object),
                ContentType.APPLICATION_JSON);
        httpPost.setEntity(body);
        return manageRequest(httpPost, type);
    }

    protected <T> HttpResponse<T> doDelete(String path, Header[] headers, Type type) {
        HttpDelete httpDelete = new HttpDelete(baseUrl + "/" + path);
        if (headers != null) {
            httpDelete.setHeaders(headers);
        }
        return manageRequest(httpDelete, type);
    }

    protected <T> HttpResponse<T> doPut(String path, Header[] headers, T object, Type type) {
        HttpPut httpPut = new HttpPut(baseUrl + "/" + path);
        if (headers != null) {
            httpPut.setHeaders(headers);
        }
        if(object==null){
            return manageRequest(httpPut, type);
        }
        StringEntity body = new StringEntity(
                gs.toJson(object),
                ContentType.APPLICATION_JSON);
        httpPut.setEntity(body);
        return manageRequest(httpPut, type);
    }

    private <T> HttpResponse<T> manageRequest(HttpRequestBase executionTarget, Type type) {
        try (CloseableHttpResponse response = client.execute(executionTarget)) {
            if (response.getStatusLine().getStatusCode() >= 200 && response.getStatusLine().getStatusCode() < 300 && response.getStatusLine().getStatusCode() != 204) {
                HttpEntity entity = response.getEntity();
                if (entity != null) {
                    String result = EntityUtils.toString(entity);
                    if (entity.getContentType().getValue().contains("json")) {
                        return new HttpResponse(response.getStatusLine().getStatusCode(), response.getStatusLine().getReasonPhrase(), response.getAllHeaders(), gs.fromJson(result, type));
                    } else {
                        return new HttpResponse(response.getStatusLine().getStatusCode(), response.getStatusLine().getReasonPhrase(), response.getAllHeaders(), result);
                    }
                }
            }
            if(response.getEntity()==null){
                return new HttpResponse(response.getStatusLine().getStatusCode(), response.getStatusLine().getReasonPhrase(), response.getAllHeaders());
            }
            return new HttpResponse(response.getStatusLine().getStatusCode(), EntityUtils.toString(response.getEntity()), response.getAllHeaders());
        } catch (ClientProtocolException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
        return null;
    }
}
