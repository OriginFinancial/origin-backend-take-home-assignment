package com.ogs.origin.exception;

public class OriginException extends Exception {

    public OriginException(String message) {
        super(message);
    }

    public OriginException(String message, Throwable cause) {
        super(message, cause);
    }
}
