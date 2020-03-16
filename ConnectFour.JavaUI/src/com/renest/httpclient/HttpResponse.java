package com.renest.httpclient;

import org.apache.http.Header;

public class HttpResponse<T> {
    final private int statusCode;
    final private String reasonPhrase;
    final private Header[] headers;
    final private T data;

    public HttpResponse(int statusCode, String reasonPhrase, Header[] headers) {
        this(statusCode, reasonPhrase, headers, null);
    }

    public HttpResponse(int statusCode, String reasonPhrase, Header[] headers, T data) {
        this.statusCode = statusCode;
        this.reasonPhrase=reasonPhrase;
        this.headers = headers;
        this.data = data;
    }

    public int getStatusCode() {
        return statusCode;
    }

    public String getReasonPhrase() {
        return reasonPhrase;
    }

    public Header[] getHeaders() {
        return headers;
    }

    public T getData() {
        return data;
    }
}
