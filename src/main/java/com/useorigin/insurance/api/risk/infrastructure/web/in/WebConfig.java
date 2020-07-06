package com.useorigin.insurance.api.risk.infrastructure.web.in;

import com.useorigin.insurance.api.risk.application.in.command.StringToMaritalStatusConverter;
import com.useorigin.insurance.api.risk.application.in.command.StringToOwnershipStatusConverter;
import org.springframework.context.annotation.Configuration;
import org.springframework.format.FormatterRegistry;
import org.springframework.web.servlet.config.annotation.WebMvcConfigurer;

@Configuration
public class WebConfig implements WebMvcConfigurer {

    @Override
    public void addFormatters(FormatterRegistry registry) {
        registry.addConverter(new StringToOwnershipStatusConverter());
        registry.addConverter(new StringToMaritalStatusConverter());
    }
}
