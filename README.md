# MASA Yellow Leads Extractor

Professional business data extraction tool for scraping contact information from Yellow Pages directories across 30+ countries worldwide.

![Application Screenshot](docs/images/screenshot-main.png)

## Overview

MASA Yellow Leads Extractor is a Windows desktop application built with .NET Framework 4.8. It automates the extraction of business contact data from over 45 Yellow Pages websites, covering major markets across Europe, North America, South America, Asia, Africa, and Oceania.

## Key Features

- Multi-country support with 45+ Yellow Pages sources
- Extracts business name, address, city, state, zip code, phone, fax, website, and email
- Built-in proxy support with free proxy list integration
- Multi-language interface (English, Italian, German, French, Spanish)
- Export to CSV, TXT, and Excel formats
- Embedded Chromium browser for manual navigation
- Data grid with selection, filtering, and batch operations

## Supported Countries

| Region | Countries | Sources |
|--------|-----------|---------|
| Europe | Italy, France, Germany, UK, Spain, Austria, Switzerland, Belgium, Holland, Czech Republic, Slovakia, Norway, Denmark, Sweden, Poland, Portugal, Ireland, Greece, Romania, Albania, Hungary, Finland | PagineGialle, GelbeSeiten, Yell, PagesJaunes, PaginasAmarillas, Herold, Local.ch, GoldenPages, Gulesider, Zlatestranky, and more |
| North America | USA, Canada, Mexico | YellowPages.com, YellowPages.ca, PaginasAmarillas.com |
| South America | Brazil, Chile | GuiaMais, Amarillas.cl |
| Asia & Middle East | Turkey, Japan, Pakistan, India, UAE, Philippines | Yelp, Lookup.pk, YellowPages.in |
| Africa | Egypt, South Africa | YellowPages.com.eg, Yep.co.za |
| Oceania | Australia, New Zealand | YellowPages.com.au, Yellow.co.nz |

## Requirements

- Windows 10 or later
- .NET Framework 4.8
- Visual Studio 2019 or later (for building from source)

## Installation

### Pre-built Release

Download the latest release from the [Releases](../../releases) page and extract to any folder.

### Build from Source

1. Clone the repository:

   ```bash
   git clone https://github.com/XREFS0/MASA-Yellow-Leads-Extractor.git
   ```

2. Open `MASAYellowLeadsExtractor.sln` in Visual Studio 2019 or later.

3. Restore NuGet packages if prompted.

4. Set build configuration to **Release**.

5. Build the solution (`Ctrl+Shift+B`).

6. The output will be in `MASAYellowLeadsExtractor/bin/Release/`.

**Note:** The third-party DLL dependencies (EO.WebBrowser, Krypton Toolkit, Newtonsoft.Json) must be placed in the build output directory. These are not included in the source repository due to licensing restrictions.

## Usage

1. Launch `MASA Yellow Leads Extractor.exe`
2. Select a country from the **Websites** menu
3. The embedded browser navigates to the selected Yellow Pages site
4. Enter search criteria in the browser
5. Click **Get data** to begin extraction
6. Review extracted results in the data grid
7. Use **Export** to save data as CSV, TXT, or Excel

### Proxy Configuration

Navigate to **Settings** to configure proxy options:

- Automatically fetch free proxy lists
- Load a custom proxy list
- Use a single proxy server
- Configure proxy authentication (username/password)

## Architecture

```
MASAYellowLeadsExtractor/
├── MASAYellowLeadsExtractor.sln
├── MASAYellowLeadsExtractor/
│   ├── MASAYellowLeadsExtractor.csproj
│   ├── MASAYellowLeadsExtractor.ico
│   ├── app.config
│   ├── HTTPScraper.cs              HTTP request utilities
│   ├── WebScraper.cs               Browser-based scraping engine
│   ├── ExcelDocument.cs            Excel export via COM Interop
│   ├── ProxyServer.cs              Proxy rotation and management
│   ├── Microsoft/                  Office Interop type stubs
│   ├── Properties/                 Assembly metadata
│   └── App/
│       ├── Program.cs              Entry point
│       ├── MainForm.cs             Primary UI and orchestration
│       ├── Settings.cs             Application configuration
│       ├── Languages.cs            Internationalization
│       ├── ExportManager.cs        Export pipeline
│       ├── DataItem.cs             Data model
│       ├── DataScrapers/           Country-specific extractors (37)
│       └── LinksScrapers/          Country-specific link parsers (40)
├── docs/images/                    Documentation assets
├── masa-page.html                  Embedded landing page
├── logo.jpg                        Application logo
└── .gitignore
```

## Technical Details

| Component | Technology |
|-----------|------------|
| Framework | .NET Framework 4.8 |
| UI | Windows Forms + Krypton Toolkit |
| Browser Engine | EO.WebBrowser (Chromium) |
| JSON | Newtonsoft.Json |
| Export | CSV, TXT, Microsoft Excel (COM Interop) |
| Build | MSBuild / Visual Studio |

## Contributing

This is a personal project. For bug reports or feature requests, please open an issue on the [GitHub repository](https://github.com/XREFS0/MASA-Yellow-Leads-Extractor/issues).

## License

All rights reserved. Copyright 2026 XREFS0.

This software is provided for educational and legitimate business research purposes only. Users are responsible for complying with the Terms of Service of any website accessed through this tool. The author is not responsible for any misuse of this software.

## Author

**XREFS0** - [GitHub](https://github.com/XREFS0)
