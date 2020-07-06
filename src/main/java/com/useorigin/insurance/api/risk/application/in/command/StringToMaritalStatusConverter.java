package com.useorigin.insurance.api.risk.application.in.command;

import com.useorigin.insurance.api.risk.domain.MaritalStatus;
import org.springframework.core.convert.converter.Converter;

public class StringToMaritalStatusConverter implements Converter<String, MaritalStatus> {

    @Override
    public MaritalStatus convert(String source) {
        return MaritalStatus.valueOf(source);
    }
}
