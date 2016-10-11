<?xml version="1.0" encoding="utf-8"?>
<!-- Designed by Yves Tremblay of ProgiNet Inc. and SBG International Inc. -->
<!-- Updated by Eric Hexter for ccnet - codecampserver -->
<!-- Updated by Roby Van Damme for use with msbuild and TeamCity-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns="http://www.w3.org/1999/xhtml">
  <xsl:output method="html" version="4.0" indent="yes" />
  <xsl:param name="applicationPath" select="'.'" />

  <xsl:variable name="stylecop.root" select="StyleCopViolations" />
  <xsl:variable name="unique.source" select="$stylecop.root/Violation[not(@Source = preceding-sibling::Violation/@Source)]" />

  <xsl:template match="/">
    <html><body>
    <div id="stylecop-report">
      <script type="text/javascript" src="Resources/report.js">
	function x() {}
      </script>
      <link rel="stylesheet" type="text/css" href="Resources/stylecop.css" />
     
      <div class="header">
        <div class="headertext">
          StyleCop 4.4 Code Analysis Report
        </div>
      </div>
      <div class="wrapper">
        <div class="legend">
          <div>
            Total Violations: <xsl:value-of select="count(//Violation)"/><br />
          </div>
        </div>
        <table class='results-table'>
          <thead>
            <tr>
              <th scope='col'></th>
              <th scope='col'></th>
              <th scope='col'>Source File</th>
              <th scope='col'>Violations</th>
            </tr>
          </thead>
          <tbody>
            <xsl:for-each select="$unique.source">
              <xsl:call-template name="print-module" />
            </xsl:for-each>
          </tbody>
        </table>
      </div>
      <!--<xsl:apply-templates select="$stylecop.root" />-->
    </div>
    </body></html>
  </xsl:template>

  <xsl:template name="print-module">
    <xsl:variable name="module.id" select="generate-id()" />
    <xsl:variable name="source" select="./@Source"/>

    <tr class="clickablerow" onclick="toggle('{$module.id}', 'img-{$module.id}')">
      <td style="width: 10px">
        <img id="img-{$module.id}" src="Resources/plus.png" />
      </td>
      <td style="width: 16px">
        <img src="Resources/error.png" />
      </td>
      <td>
        <xsl:value-of select="$source" />
      </td>
      <td>
        <xsl:value-of select="count(//Violation[@Source=$source])" />
      </td>
    </tr>
    <xsl:call-template name="print-module-error-list">
      <xsl:with-param name="module.id" select="$module.id"/>
      <xsl:with-param name="source" select="$source"/>
    </xsl:call-template>
  </xsl:template>

  <xsl:template name="print-module-error-list">
    <xsl:param name="module.id" />
    <xsl:param name="source" />

    <tr id="{$module.id}" class="errorlist" style="display: none">
      <td></td>
      <td colspan="6">
        <table cellpadding="2" cellspacing="0" width="100%" class="inner-results">
          <thead>
            <tr class="inner-header">
              <th scope='col'></th>
              <th scope='col'>Line</th>
              <th scope='col'>Violation Message</th>
            </tr>
          </thead>
          <tbody>
            <xsl:for-each select='$stylecop.root/Violation[@Source = $source]'>
              
              <xsl:sort select="@LineNumber" data-type="number"/>
              
              <xsl:variable name="message.id" select="generate-id()" />
              <xsl:variable name="rule.id" select="@RuleId" />
              <xsl:variable name="rule.rule" select="@Rule" />
              <xsl:variable name="rule.namespace" select="@RuleNamespace" />
              <xsl:variable name="section" select="@Section" />
             
              <tr class="clickablerow" onclick="toggle('{$module.id}-{$message.id}', 'img-{$module.id}-{$message.id}')">
                <td style="width: 10px">
                  <img id="img-{$module.id}-{$message.id}" src="Resources/plus.png" />
                </td>
                <td>
                  <xsl:value-of select="@LineNumber" />
                </td>
                <td>
                  <xsl:value-of select="text()" />
                </td>
              </tr>
              <tr id="{$module.id}-{$message.id}" style="display: none">
                <td></td>
                <td colspan="5">
                  <div class="legend">
                    <div>
                      <table cellpadding="2" cellspacing="0" width="100%" class="inner-rule-description">
                        <tr>
                          <td>
                            <b>Rule:</b>
                          </td>
                          <td>
                            <xsl:value-of select="$rule.rule" />
                          </td>
                        </tr>
                        <tr>
                          <td>
                            <b>Rule Id:</b>
                          </td>
                          <td>
                            <xsl:value-of select="$rule.id" />
                          </td>
                        </tr>
                        <tr>
                          <td>
                            <b>Rule Namespace:</b>
                          </td>
                          <td>
                            <xsl:value-of select="$rule.namespace" />
                          </td>
                        </tr>
                        <tr>
                          <td>
                            <b>Section:</b>
                          </td>
                          <td>
                            <xsl:value-of select="$section" />
                          </td>
                        </tr>
                      </table>
                    </div>
                  </div>
                </td>
              </tr>
            </xsl:for-each>
          </tbody>
        </table>
      </td>
    </tr>
  </xsl:template>

</xsl:stylesheet>


