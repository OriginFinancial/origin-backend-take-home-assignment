package com.useorigin.insurance.api.risk.application.in.command;

import com.useorigin.insurance.api.risk.domain.OwnershipStatus;
import org.springframework.core.convert.converter.Converter;

public class StringToOwnershipStatusConverter implements Converter<String, OwnershipStatus> {

    @Override
    public OwnershipStatus convert(String source) {
        return OwnershipStatus.valueOf(source);
    }
}
