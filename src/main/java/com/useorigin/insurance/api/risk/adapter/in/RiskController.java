package com.useorigin.insurance.api.risk.adapter.in;

import com.useorigin.insurance.api.risk.application.in.command.RiskProfileCreationCommand;
import com.useorigin.insurance.api.risk.application.service.RiskService;
import com.useorigin.insurance.api.risk.infrastructure.web.out.RiskProfileResource;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

import javax.validation.Valid;

import static org.springframework.http.HttpStatus.CREATED;

@RestController
public class RiskController {

    private final RiskService service;

    public RiskController(RiskService service) {
        this.service = service;
    }

    @PostMapping("/insurances/risk")
    private ResponseEntity<RiskProfileResource> risk(@RequestBody @Valid RiskProfileCreationCommand command) {

        RiskProfileResource riskProfile = service.createProfile(command);

        return ResponseEntity.status(CREATED).body(riskProfile);
    }
}
